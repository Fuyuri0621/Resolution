using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using Unity.Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class PlayerStats
{
    public int hp = 100;
    public bool superarmor = false;

    public PlayerStats(PlayerStats stats)
    {
        this.hp = stats.hp;

    }


}

public class Playercontrol1 : MonoBehaviour, IDamageable
{

    public AllPlayerCharacter thisCharacter;
    Animator animator;
    Rigidbody rb;
    Transform playerTransform;

    public enum PlayerPosture
    {
        Crouch,
        Stand,
        Midair,
    };
    public PlayerPosture playerPosture = PlayerPosture.Stand;

    float crouchThreshold = (-1f);
    float idleThreshold = 0f;
    float walkThreshold = 1f;
    float runThreshold = 2f;
    float sprintThreshold = 3f;
    public enum LocomotionState
    {
        Crouch,
        Idle,
        Walk,
        Run,
        Sprint,
    };
    public LocomotionState locomotionState = LocomotionState.Idle;

    public enum ArmState
    {
        Normal,
        Aim
    };
    public ArmState armState = ArmState.Normal;

    public PlayerStats stats;
    public Vector3 inputVector;

    //  float crouchSpeed = 1.5f;
    // float walkspeed = 2.5f;
    // float runspeed = 5.5f;

    //  [SerializeField] float dashlevel = 2.5f;
    [SerializeField] bool needdash = false;
    float dashcd = 3;

    bool isCrouch = false;
    bool isWalk = false;
    bool isAiming = false;
    bool isSprint = false;

    int postureHash;
    int movevector_xHash;
    int movevector_yHash;
    int isbattleHash;
    public Vector3 inputSmooth;
    Quaternion finalrotate;
    private Vector3 finalMoveVector;
    public Vector3 playerMovement;
    public AudioClip hitSound;

    public GameObject Cam;
    float mousex, mousey;
    private Quaternion rotationEuler;
    private Vector3 cameraPosition;
    [SerializeField]
    Vector3 camareoffset;
    [SerializeField]
    [Header("相機距離")]
    public float distence = 5;
    [SerializeField]
    [Header("最大高度")]
    [Range(-89, 89)]
    public float maxy = 89;
    [SerializeField]
    [Header("最低高度")]
    [Range(-89, 89)]
    public float miny = -45;
    [SerializeField]
    [Header("視角速度")]
    public float CamSpeed = 200;
    private float fy = 0;

    FriednlyAI friednlyAI;
    KnockbackControl knockbackControl;
    follow follow;
    StatusBar statusBar;
    private void Awake()
    {
        statusBar = GameObject.Find("PlayerState").GetComponent<StatusBar>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerTransform = transform;
        friednlyAI = GetComponent<FriednlyAI>();
        follow = GetComponent<follow>();
        knockbackControl = GetComponent<KnockbackControl>();

    }
    void Start()
    {
        postureHash = Animator.StringToHash("PlayerPosture");
        movevector_xHash = Animator.StringToHash("movevector_x");
        movevector_yHash = Animator.StringToHash("movevector_y");
        isbattleHash = Animator.StringToHash("isbattle");
        Cursor.lockState = CursorLockMode.Locked;



    }


    void Update()
    {


        if (GameManager.Instance.controlingCharacter == thisCharacter)
        {
            Cam.SetActive(true);
        }
        else
        {
            Cam.SetActive(false);
        }


        if (GameManager.Instance.IsGameRunning && GameManager.Instance.controlingCharacter == thisCharacter)
        {
            Camcontrol();
            AvoidCrossWall();
            if (currentTarget != null) { CheckLockTarget(); }

            if (GameManager.Instance.IsControlling)
            {

                inputSmooth = Vector3.Lerp(inputSmooth, inputVector, 6 * Time.deltaTime);

                UpdateMoveDirection();

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("battlemove") || animator.GetCurrentAnimatorStateInfo(0).IsName("move 0") || animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
                { RotateToDirection(playerMovement); }

                DashCheck(playerMovement);


                SwitchPlayerStates();
                SetupAnimator();

                if (inputVector != Vector3.zero)
                {
                    if (animator.GetBool("ismove"))

                        animator.SetBool("ismove", true);




                }
                else { animator.SetBool("ismove", false); }



            }
        }

    }

    private void DashCheck(Vector3 direction)
    {
        if (needdash)
        {

            direction.y = 0f;

            Quaternion _newRotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = _newRotation;


            animator.CrossFade("Dodge", 0.1f);
            needdash = false;
            dashcd = 1f;
        }

        if (dashcd > 0)
            dashcd = Math.Min(dashcd, dashcd - Time.deltaTime);

        if (dashcd < 0)
            dashcd = 0;
    }

    public void RotateTo()
    {
        if (currentTarget != null&& inputVector.z>0.2)
        {
            playerMovement = Quaternion.AngleAxis(6, Vector3.up) * playerMovement;
        }
        RotateToDirection(playerMovement);
    }
    private void RotateToDirection(Vector3 direction)
    {
        direction.y = 0f;
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, direction.normalized, 16 * Time.deltaTime, .1f);
        Quaternion _newRotation = Quaternion.LookRotation(desiredForward);

        transform.rotation = _newRotation;
    }


    #region 輸入相關
    public void OnMove(InputValue value)
    {
        inputVector.x = value.Get<Vector2>().x;
        inputVector.z = value.Get<Vector2>().y;

        if (GameManager.Instance.IsControlling)
        {

        }
        else
        {
            animator.SetFloat("PlayerPosture", 0);
            animator.SetFloat("movevector_x", 0);
            animator.SetFloat("movevector_y", 0);
        }
    }

    public void OnDash(InputValue value)
    {
        if (GameManager.Instance.IsthisCtrolingCharacter(thisCharacter))
        {
            if (GameManager.Instance.isBattling && dashcd == 0 && !gameObject.GetComponent<ComboManager>().isOnNeceTime)
            {
                needdash = true;
            }
        }
    }
    public void OnCallmenu(InputValue value)
    {

        if (value.isPressed)
        {
            GameManager.Instance.PauseGame();

        }
    }

    public virtual void UpdateMoveDirection()
    {
        if (inputVector.magnitude <= 0.01)
        {
            inputVector = Vector3.Lerp(inputVector, Vector3.zero, 6 * Time.deltaTime);
            return;
        }

        var right = Cam.transform.right;
        right.y = 0;
        var forward = Quaternion.AngleAxis(-90, Vector3.up) * right;
       
        playerMovement = (inputSmooth.x * right) + (inputSmooth.z * forward);

        if (currentTarget != null)
        {

            playerMovement = Quaternion.AngleAxis(15, Vector3.up) * playerMovement;
        }

    }
    #endregion
    #region 動畫相關
    void SwitchPlayerStates()
    {
        if (isCrouch)
        {
            playerPosture = PlayerPosture.Crouch;
        }
        else
        {
            playerPosture = PlayerPosture.Stand;
        }

        if (inputVector.magnitude == 0)
        {
            locomotionState = LocomotionState.Idle;
        }
        else if (isCrouch)
        {
            locomotionState = LocomotionState.Crouch;
        }
        else if (isSprint)
        {
            locomotionState = LocomotionState.Sprint;
        }
        else if (isWalk)
        {
            locomotionState = LocomotionState.Walk;
        }
        else
        { locomotionState = LocomotionState.Run; }

        if (isAiming)
        {
            armState = ArmState.Aim;
        }
        else
        {
            armState = ArmState.Normal;
        }
    }

    void SetupAnimator()
    {
        if (locomotionState == LocomotionState.Idle)
        {
            if (animator.GetFloat(postureHash) > 0.001f)
                animator.SetFloat(postureHash, idleThreshold, 0.1f, Time.deltaTime);

        }
        else if (locomotionState == LocomotionState.Crouch)
        {
            animator.SetFloat(postureHash, crouchThreshold, 0.1f, Time.deltaTime);

        }
        else if (locomotionState == LocomotionState.Walk)
        {
            animator.SetFloat(postureHash, walkThreshold, 0.1f, Time.deltaTime);

        }
        else if (locomotionState == LocomotionState.Run)
        {
            animator.SetFloat(postureHash, runThreshold, 0.1f, Time.deltaTime);

        }
        else
        {
            animator.SetFloat(postureHash, sprintThreshold, 0.1f, Time.deltaTime);
        }

        if (armState == ArmState.Normal)
        {

        }

        if (inputVector != Vector3.zero)
        {
            animator.SetFloat("movevector_x", playerMovement.x, 0.1f, Time.deltaTime);
            animator.SetFloat("movevector_y", playerMovement.z, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("movevector_x", 0);
            animator.SetFloat("movevector_y", 0);
            if (isSprint) { isSprint = false; }
        }
    }
    #endregion

    private void OnAnimatorMove()
    {
        if (GameManager.Instance.IsGameRunning && GameManager.Instance.IsControlling)
        {
            Vector3 targetPosition = animator.rootPosition;
            Vector3 targetVelocity = (targetPosition - transform.position) / (Time.deltaTime + 0.001f);

            targetVelocity.y = rb.linearVelocity.y;

            rb.linearVelocity = targetVelocity;

            finalMoveVector = animator.deltaPosition;
            finalrotate = animator.deltaRotation;
        }
    }

    private bool canSwitchTarget = true;
    private float switchCooldown = 0.5f;
    private float lastSwitchTime = 0f;

    public float switchinputThreshold = 50f;
    public void OnLook(InputValue value)
    {

        if (GameManager.Instance.IsGameRunning)
        {
            mousex += value.Get<Vector2>().x * CamSpeed * Time.deltaTime;
            fy = mousey - value.Get<Vector2>().y * CamSpeed * Time.deltaTime;

            if (currentTarget != null && canSwitchTarget)
            {
                float now = Time.time;

                if (value.Get<Vector2>().x > switchinputThreshold)
                {
                    SwitchTarget(1);
                    canSwitchTarget = false;
                    lastSwitchTime = now;
                }
                else if (value.Get<Vector2>().x < -switchinputThreshold)
                {
                    SwitchTarget(-1);
                    canSwitchTarget = false;
                    lastSwitchTime = now;
                }
            }
            if (!canSwitchTarget && Mathf.Abs(value.Get<Vector2>().x) < 5f)
            {

                if (Time.time - lastSwitchTime >= switchCooldown)
                {
                    canSwitchTarget = true;
                }
            }
        }
    }

    public float lockondistance = 2f;
    void Camcontrol()
    {
        if (!(fy <= miny || fy >= maxy))
        {
            mousey = fy;
        }

        if (currentTarget != null)
        {
            Vector3 direction = (transform.position - currentTarget.position).normalized;
            Vector3 right = Vector3.Cross(Vector3.up, direction);
            cameraPosition = transform.position + direction * lockondistance + Vector3.up * 1f - right * 1.5f;

        }
        else
        {
            mousex %= 360;
            mousey %= 90;
            rotationEuler = Quaternion.Euler(mousey, mousex, 0);

            cameraPosition = rotationEuler * new Vector3(0, 0, -distence) + (transform.position + new Vector3(0, 1, 0));
            Cam.transform.rotation = rotationEuler;
        }
        Cam.transform.position = cameraPosition + camareoffset;

    }

    private void AvoidCrossWall()
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up, Cam.transform.position + new Vector3(0, -0.1f, 0), out hit))
        {
            if (hit.transform.name != "MainCamera" && hit.collider.tag == "Obstacle")
            {
                Cam.transform.position = hit.point - (Vector3.ClampMagnitude(hit.point - (transform.position + new Vector3(0, 1.5f, 0)), 0.5f));
            }
        }
        else if (Physics.Linecast(transform.position + Vector3.up, Cam.transform.position, out hit))
        {
            if (hit.transform.name != "MainCamera" && hit.collider.tag == "Obstacle")
            {
                Cam.transform.position = hit.point - (Vector3.ClampMagnitude(hit.point - (transform.position + new Vector3(0, 1.5f, 0)), 0.5f));
            }
        }
    }

    public void Switch()
    {
        if (!GameManager.Instance.IsthisCtrolingCharacter(thisCharacter))
        {
            if (GameManager.Instance.isBattling)
            {
                friednlyAI.enabled = true;
            }
            follow.enabled = true;
        }
        else
        {
            if (GameManager.Instance.isBattling)
            {
                friednlyAI.enabled = false;
            }
            follow.enabled = false;
        }
        if (currentTarget != null)
        {
            UnlockTarget();
        }
    }


    public void OnRun(InputValue value)
    {
        if (isCrouch) { isCrouch = false; }
        if (isSprint) { isSprint = false; }

        isWalk = !isWalk;

        if (isWalk)
        {
            locomotionState = LocomotionState.Run;
        }
        else locomotionState = LocomotionState.Idle;
    }
    public void OnSprint(InputValue value)
    {
        if (isCrouch) { isCrouch = false; }
        if (isWalk) { isWalk = false; }

        isSprint = !isSprint;

        if (isSprint)
        {
            locomotionState = LocomotionState.Sprint;
        }
        else locomotionState = LocomotionState.Idle;
    }

    public void OnCrouch(InputValue value)
    {
        if (isWalk) { isWalk = false; }
        if (isSprint) { isSprint = false; }

        isCrouch = !isCrouch;

        if (isCrouch)
        {
            locomotionState = LocomotionState.Crouch;
        }
        else locomotionState = LocomotionState.Idle;
    }

    public void StartBattle()
    {
        animator.SetFloat(isbattleHash, 1);
        animator.Play("Spear_Equip");
        if (GameManager.Instance.controlingCharacter != thisCharacter)
        {

            friednlyAI.enabled = true;
        }
    }
    public void EndBattle()
    {
        animator.SetFloat(isbattleHash, 0);
        animator.Play("Spear_Unequip");
        if (GameManager.Instance.controlingCharacter != thisCharacter)
        {
            GetComponent<follow>().enabled = true;
            friednlyAI.enabled = true;
        }
    }

    public void TakeDamage(int damage, float knockback, Transform attackform)
    {
        if (stats.superarmor) return;

        stats.hp -= damage;

        Vector3 enemyLocalPos = playerTransform.InverseTransformPoint(attackform.position);
        enemyLocalPos.Normalize();
        knockbackControl.Knockback(knockback, -enemyLocalPos);

        statusBar.UpdateSlider("PlayerHP", stats.hp);

        if (stats.hp <= 0)
        {
            GameManager.Instance.GameOver();
        }

        animator.CrossFade("BeAttack_player", 0.1f);
    }

    void IDamageable.TakeDamage(int damage, int stun, float knockback, AllPlayerCharacter attacker)
    {
        return;
    }

    public void GetSuperArmor()
    {
        stats.superarmor = true;
    }
    public void ReturnSuperArmor()
    {
        stats.superarmor = false;
    }

    #region 鎖定敵人
    private Transform currentTarget;
    private List<Transform> availableTargets = new List<Transform>();
    private int currentIndex = 0;
    public float lockOnRadius = 15f;

    public void OnLockon()
    {
        if (GameManager.Instance.IsthisCtrolingCharacter(thisCharacter))
        {
            if (currentTarget == null)
            {
                TryLockOn();
            }
            else
            {
                UnlockTarget();
            }

            if (currentTarget != null)
            {
                Cam.GetComponent<CinemachineCamera>().LookAt = currentTarget;
            }
            else { Cam.GetComponent<CinemachineCamera>().LookAt = null; }
        }
    }
    void TryLockOn()
    {
        availableTargets = FindTargetsInRadius();
        if (availableTargets.Count == 0) return;

        currentIndex = 0;
        currentTarget = availableTargets[currentIndex];
        EnemyControl e = currentTarget.GetComponent<EnemyControl>();
        if (e != null) { e.islocked = true; }

        Debug.Log("LOCKON");
    }

    void UnlockTarget()
    {
        EnemyControl e = currentTarget.GetComponent<EnemyControl>();
        if (e != null) { e.islocked = false; }
        currentTarget = null;
        availableTargets.Clear();
        Cam.GetComponent<CinemachineCamera>().LookAt = null;
        Debug.Log("UNLOCK");
    }
    void SwitchTarget(int direction)
    {
        
        if (availableTargets.Count == 0) return;
        EnemyControl e = currentTarget.GetComponent<EnemyControl>();
        if (e != null) { e.islocked = false; }
        currentIndex = (currentIndex + direction + availableTargets.Count) % availableTargets.Count;
        currentTarget = availableTargets[currentIndex];
        e = currentTarget.GetComponent<EnemyControl>();
        if (e != null) { e.islocked = true; }
        Cam.GetComponent<CinemachineCamera>().LookAt = currentTarget;
    }
    List<Transform> FindTargetsInRadius()
    {
        List<Transform> result = new List<Transform>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (enemy.GetComponent<EnemyControl>() != null)
            {
                if (dist <= lockOnRadius && enemy.GetComponent<EnemyControl>().stats.hp > 0)
                    result.Add(enemy.transform);
            }else if(dist <= lockOnRadius) { result.Add(enemy.transform); }
        }

        result.Sort((a, b) =>
        {
            Vector3 dirA = a.position - Cam.transform.position;
            Vector3 dirB = b.position - Cam.transform.position;
            float angleA = Vector3.Angle(Cam.transform.forward, dirA);
            float angleB = Vector3.Angle(Cam.transform.forward, dirB);
            return angleA.CompareTo(angleB);
        });

        return result;
    }

    void CheckLockTarget()
    {
        if (currentTarget.GetComponent<BossControl>() != null) { return; }
        if (currentTarget.GetComponent<EnemyControl>().stats.hp <= 0)
        {
            availableTargets = FindTargetsInRadius();
            if (availableTargets.Count >= 1)
            { SwitchTarget(1); }
            else { UnlockTarget(); }
        }

    }
    #endregion
}