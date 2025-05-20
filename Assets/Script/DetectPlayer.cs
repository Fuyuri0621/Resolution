using Micosmo.SensorToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum State
{
    idle,
    patrol,
    attack,
    pursuit,
    dead,
}

public class DetectPlayer : MonoBehaviour
{
    // Serialized fields
    [SerializeField] private DetectPlayer[] alertGroup;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private bool canBattle = true;
    [SerializeField] private GameObject failPanel;
    [SerializeField] private Transform[] PatrolPath;
    [SerializeField] private int tragetPatrolPath = 0;
    [SerializeField] private float changePatrolCD = 5f;
    [SerializeField] private int HP = 100;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider suspiciousSlider;
    [SerializeField] private Slider alertnessSlider;
    [SerializeField] private GameObject backCanvas;
    [SerializeField] private GameObject sliderCanvas;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform backattackpos;
    [SerializeField] private Sensor targetSensor;
    [SerializeField] private Sensor BackSensor;
    [SerializeField] private int suspiciousRate = 5;
    [SerializeField] private float suspicious = 0f;
    [SerializeField] private float alertness = 0f;
    [SerializeField] private float releaseTime = 5f;
    [SerializeField] private NavToTarget nav;
    [SerializeField] private RagDollControl dollControl;
    [SerializeField] private State state = State.idle;

    // Private fields
    private float attackCD = 0f;
  //  private bool IsAttacking = false;
    private Vector3 lastSuspiciousLocation;
    private GameObject target;
    private bool isDead = false;
    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    void Start()
    {
        state = State.patrol;
    }

    void Update()
    {
        UpdateBackSensor();
        UpdateTargetSensor();

        switch (state)
        {
            case State.idle:
                // Idle behavior
                break;
            case State.patrol:
                PatrolMode();
                break;
            case State.attack:
                AttackMode();
                break;
            case State.pursuit:
                PursuitMode();
                break;
            case State.dead:
                HandleDeath();
                break;
        }

        UpdateSliders();
    }

    private void UpdateBackSensor()
    {
        if (BackSensor.GetNearestDetection() != null)
        {
            if (GameManager.Instance.IsControlling)
            {
                backCanvas.SetActive(true);
              //  BackSensor.GetNearestDetection().GetComponent<Playercontrol1>().AttackCd = 1;
            }
        }
        else
        {
            backCanvas.SetActive(false);
        }
    }

    private void UpdateTargetSensor()
    {
        if (targetSensor.GetNearestDetection() != null)
        {
            if (GameManager.Instance.IsControlling)
            {
                target = targetSensor.GetNearestDetection();
            }
        }
        else if(!canBattle)
        {
            target = null;
        }
    }

    private void AttackMode()
    {
        nav.target = transform.position;

        if (attackCD == 0)
        {
            attackCD = 3f;
        }

        if (attackCD <= 1f)
        {
            state = State.pursuit;
        }

        attackCD = Mathf.Max(0f, attackCD - Time.deltaTime);
    }

    private void PursuitMode()
    {
        if (releaseTime == 0&&!canBattle)
        {
            state = State.patrol;
         //   GameManager.Instance.RemovetBattleEmemies(this);
            return;
        }

        if (target != null)
        {
            lastSuspiciousLocation = target.transform.position;
            nav.target = target.transform.position;
            releaseTime = 3f;
        }
        else
        {
            
            releaseTime = Mathf.Max(0f, releaseTime - Time.deltaTime);
        }

        if (target != null && attackCD == 0 && Vector3.Distance(transform.position, target.transform.position) < 1.5f)
        {
            state = State.attack;
        }
    }

    private void PatrolMode()
    {
        if (target != null && GameManager.Instance.IsControlling)
        {
            if (canBattle)
            {
                suspicious = 100f;
                alertness = 100f;
                lastSuspiciousLocation = target.transform.position;
                ActiveAlertGroup();
                state = State.pursuit;
            }

            AddSuspicious();
            lastSuspiciousLocation = target.transform.position;
            releaseTime = 3f;
        }
        else
        {
            releaseTime = Mathf.Max(0f, releaseTime - Time.deltaTime);

            if (releaseTime == 0)
            {
                ReduceAlertness();

                if (alertness == 0 && releaseTime == 0)
                {
                    ReduceSuspicious();
                    UpdatePatrolPath();
                }
            }
        }
    }

    private void UpdatePatrolPath()
    {
        if (Vector3.Distance(PatrolPath[tragetPatrolPath].position, transform.position) < 1.3f)
        {
            changePatrolCD = Mathf.Max(0f, changePatrolCD - Time.deltaTime);

            if (changePatrolCD == 0)
            {
                tragetPatrolPath = (tragetPatrolPath + 1) % PatrolPath.Length;
                changePatrolCD = 5f;
            }
        }

        nav.target = PatrolPath[tragetPatrolPath].position;
    }

    private void AddSuspicious()
    {
        if (suspicious == 100f)
        {
            nav.target = lastSuspiciousLocation;
            AddAlertness();
            return;
        }

        suspicious = Mathf.Min(100f, suspicious + Time.deltaTime * suspiciousRate);
    }

    private void ReduceSuspicious()
    {
        suspicious = Mathf.Max(0f, suspicious - Time.deltaTime * 60f);
    }

    private void AddAlertness()
    {
        if (alertness == 100f)
        {
            if (!canBattle)
            {
                failPanel.SetActive(true);
                GameManager.Instance.PauseGame();
            }
            else
            {
             //   GameManager.Instance.AddtBattleEmemies(this);
            }

            state = State.pursuit;
            return;
        }

        alertness = Mathf.Min(100f, alertness + Time.deltaTime * suspiciousRate);
    }

    private void ReduceAlertness()
    {
        alertness = Mathf.Max(0f, alertness - Time.deltaTime * 60f);
    }

    private void UpdateSliders()
    {
      //  UpdateSlider(hpSlider, HP);
        UpdateSlider(suspiciousSlider, (int)suspicious);
        UpdateSlider(alertnessSlider, (int)alertness);
    }

    public void UpdateSlider(Slider targetSlider, int current)
    {
        targetSlider.value = current;
    }

    public void BackAttack()
    {
        if (backCanvas.activeInHierarchy)
        {
            animator.Play("BeBackAttack", 0);
            nav.target = transform.position;
            sliderCanvas.SetActive(false);
            backCanvas.SetActive(false);

            if (BackSensor.enabled)
            {
                Transform nearestDetection = BackSensor.GetNearestDetection().gameObject.transform;
                nearestDetection.position = backattackpos.position;
                nearestDetection.rotation = backattackpos.rotation;
                nearestDetection.GetComponent<Animator>().Play("BackAttack", 0);
                BackSensor.enabled = false;
            }

            state = State.dead;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("playerweapon"))
        {
            Hited();

            Vector3 pos = col.ClosestPointOnBounds(other.transform.position);
            EffectManager.Instance.SpawnHitEffect(pos);
        }
    }

    public void Hited()
    {
        animator.Play("BeAttack_enemy", 0);
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
        HP -= 10;

        if (HP <= 0)
        {
            nav.target = transform.position;
            sliderCanvas.SetActive(false);
            backCanvas.SetActive(false);
            state = State.dead;
        }
    }

    public void ActiveAlertGroup()
    {
        if (alertGroup == null) return;

        foreach (var alert in alertGroup)
        {
            alert.target = target;
            alert.lastSuspiciousLocation = target.transform.position;
            alert.nav.target = target.transform.position;
            alert.suspicious = 100f;
            alert.alertness = 100f;
        }

        state = State.pursuit;
    }

    private void HandleDeath()
    {
        if (!isDead)
        {
            col.enabled = false;
            animator.Play("BeAttackdead");
            nav.target = transform.position;
            sliderCanvas.SetActive(false);
            backCanvas.SetActive(false);
            BackSensor.enabled = false;
            targetSensor.enabled = false;
            isDead = true;

           // GameManager.Instance.RemovetBattleEmemies(this);
        }
    }
}