using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Micosmo.SensorToolkit;
using TMPro;
using Unity.VisualScripting;
using static BackpackLocalData;
using UnityEditor.Experimental.GraphView;

[Serializable]
public class EnemyStats
{

    public int hp = 100;
    public int stun = 0;
    public int damage = 1;
    public EnemyAiState state= EnemyAiState.idle;
    public bool superarmor = false;




    public EnemyStats(EnemyStats stats)
    {
        this.hp = stats.hp;
        this.stun = stats.stun;
        this.damage = stats.damage;
        this.state = stats.state;


    }


}
public enum EnemyAiState
{
    idle,
    patrol,
    pursuit,
    wander,
    attack,
    stun,
    dead
}
public class EnemyControl : MonoBehaviour,IDamageable
{
    public EnemyStats stats;
    public int maxStun=100;
    public int stunTimeRecoverRate=1;
    private float stunTimer;
    public float attackRange=0.8f;
    public float attackCooldown = 2f;
    StatusBar statusBars;
    [SerializeField] private Transform[] PatrolPath;
    private int tragetPatrolPath = 0;
    private float changePatrolCD = 5f;
    [SerializeField] private EnemyControl[] alertGroup;
    [SerializeField] GameObject sliderCanvas;
     GameObject hpBase;
    [SerializeField] private Animator animator;
    private NavToTarget nav;
    [SerializeField] private Sensor targetSensor;
    private Collider col;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] float toWanderDistance = 2;
    [SerializeField] float wanderDistance = 2f;// 隨機徘徊的最大距離
    [SerializeField] float outwanderDistance = 5f;
    [SerializeField] private GameObject target;
    KnockbackControl knockbackControl;
    float attackCD = 5;
    Rigidbody rgbd;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();
        nav = GetComponent<NavToTarget>();
        statusBars = GetComponent<StatusBar>();
        rgbd = GetComponent<Rigidbody>();
        knockbackControl = GetComponent<KnockbackControl>();

    }
    private void Start()
    {

        statusBars.UpdateSlider("hp",stats.hp);
        hpBase = sliderCanvas.transform.Find("HP_Base").gameObject;
    }

    private void Update()
    {

        if (GameManager.Instance.IsGameRunning)
        {

            switch (stats.state)
            {
                case EnemyAiState.idle:
                    // Idle behavior
                    break;
                case EnemyAiState.patrol:
                    PatrolMode();
                    break;
                case EnemyAiState.pursuit:
                    PursuitMode();
                    break;
                case EnemyAiState.wander:
                    WanderMode();
                    break;
                case EnemyAiState.attack:
                    AttackMode();
                    break;
                case EnemyAiState.stun:
                    StunMode();
                    break;
                case EnemyAiState.dead:
                    HandleDeath();
                    break;
            }

            if (target != null)
            {
                nav.targetCharacter = target.GetComponent<Playercontrol1>().thisCharacter;
            }

            if (islocked)
            {
                if (!hpBase.activeInHierarchy) { hpBase.SetActive(true); }

            }
            else if (stats.state == EnemyAiState.stun) { hpBase.SetActive(true); }
            else
            {
                if (hpBase.activeInHierarchy && Time.time > hptimer) { hpBase.SetActive(false); }
            }
        }

    }

    private void IdleMode()
    {
        if (nav.GetSpeed() != 0)
        {
            nav.SetSpeed(0);
        }
    }

    private void PatrolMode()
    {
        UpdateTargetSensor();
        UpdatePatrolPath();

        if (target != null && GameManager.Instance.IsControlling)
        {
                ActiveAlertGroup();
                GameManager.Instance.AddtBattleEmemies(this.gameObject);
           nav.needFaceplayer = true;


            animator.SetBool("isbattle",true);
            stats.state = EnemyAiState.pursuit;


        }
       
    }
    private void PursuitMode()

    {
        if(!nav.needFaceplayer)nav.needFaceplayer = true;
        if (nav.GetSpeed() != 2)
        {
            nav.SetSpeed(2);
        }
        //防止一直追
       


        if (target != null)
        {
            if (attackCD > 0) { attackCD = Math.Max(0, attackCD - Time.deltaTime); }

            Vector3 direction = transform.position - target.transform.position;
            direction = direction.normalized;
            Vector3 pursuitPosition = target.transform.position + direction * toWanderDistance;

            if (Vector3.Distance(transform.position, pursuitPosition) <= 0.3f)
            {
                stats.state = EnemyAiState.wander;
            }

            nav.target = pursuitPosition;
        }
        else { stats.state = EnemyAiState.patrol; }
    }

    private void WanderMode()
    {
        if (nav.GetSpeed() != 0.5f)
        {
            nav.SetSpeed(0.5f);
        }

        if (attackCD > 0) { attackCD = Math.Max(0, attackCD - Time.deltaTime); }

        if (target != null && attackCD == 0)
        {
            if (GameManager.Instance.IsthisCtrolingCharacter( target.GetComponent<Playercontrol1>().thisCharacter))
            {
                if (GameManager.Instance.RequestAttack())
                {
                    stats.state = EnemyAiState.attack;
                }
                else { attackCD = attackCooldown; }

            }
            else
            {
                stats.state = EnemyAiState.attack;
            }

        }

        float tragetDistance = Vector3.Distance(transform.position, target.transform.position);

        if (tragetDistance >= outwanderDistance)
        {
            stats.state = EnemyAiState.pursuit;
        }

        if (tragetDistance < 1.5f)
        {
            Vector3 direction = transform.position - target.transform.position;
            direction = direction.normalized;
            Vector3 pursuitPosition = target.transform.position + direction * toWanderDistance;
            nav.target = pursuitPosition;
        }

        if (Vector3.Distance(transform.position, nav.target) < 0.3f)
        {
            nav.target = GetNewWanderposition();
        }
    }

    private Vector3 GetNewWanderposition()
    {
        Vector3 wanderPosition;
        float angle = UnityEngine.Random.Range(0, 360);
        Vector3 offset = new Vector3(Mathf.Cos(angle) * wanderDistance, 0, Mathf.Sin(angle) * wanderDistance);
        wanderPosition = transform.position + offset;
        return wanderPosition;
    }

    private void AttackMode()
    {
        if (nav.GetSpeed()!=2)
        {
            nav.SetSpeed(2);
        }
        float tragetDistance = Vector3.Distance(transform.position, target.transform.position);

        if (tragetDistance > attackRange)
        {
            Vector3 direction = transform.position - target.transform.position;
            direction = direction.normalized;
            Vector3 pursuitPosition = target.transform.position + direction * attackRange;
            nav.target = pursuitPosition;
        }else nav.target =transform.position;

        if (tragetDistance < attackRange+0.5f)
        {
            Attack();
        }
    }
    private void StunMode()
    {
        animator.SetBool("isstun", true);
        if (stunTimer < 0)
        {
            stats.stun -= 1;
            stunTimer = 1;
            statusBars.UpdateSlider("stun", stats.stun);
        }
        stunTimer-=Time.deltaTime*stunTimeRecoverRate*10;


        if (nav.GetSpeed() != 0)
        {
            nav.SetSpeed(0);
        }

        nav.target = transform.position;


        if(stats.stun == 0)
        {
            animator.SetBool("isstun", false);
            stats.state= EnemyAiState.pursuit;
        }

    }


    private void HandleDeath()
    {
        nav.needFaceplayer = false;
        col.enabled = false;
        animator.Play("BeAttackdead");
        nav.target = transform.position;
        sliderCanvas.SetActive(false); 
        targetSensor.enabled = false;
        GameManager.Instance.RemovetBattleEmemies(this.gameObject);
        Destroy(gameObject, 5);
    }

    private void UpdatePatrolPath()
    {
        if (PatrolPath.Length == 0) {return; }
        if (Vector3.Distance(PatrolPath[tragetPatrolPath].position, transform.position) < 1.4f)
        {

            if (changePatrolCD > 0) { changePatrolCD = Mathf.Max(0f, changePatrolCD - Time.deltaTime); }

            if (changePatrolCD == 0)
            {
                tragetPatrolPath = (tragetPatrolPath + 1) % PatrolPath.Length;
                changePatrolCD = 5f;
            }
        }

        nav.target = PatrolPath[tragetPatrolPath].position;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("playerweapon"))
        {

            ComboManager c = other.GetComponentInParent<ComboManager>();
            int damage = c.currentDamageRate;
            int stun = c.currentStunRate;
            float knockback = c.currentknockbackrate;
            AllPlayerCharacter attacker = c.thisCharacter;
            TakeDamage(damage,stun,knockback,attacker);

            Vector3 pos = col.ClosestPointOnBounds(other.transform.position);
            EffectManager.Instance.SpawnHitEffect(pos);
        }
    }

    private void Attack()
    {

       
            animator.Play("Attack_enemy");
            attackCD = attackCooldown;
            stats.state = EnemyAiState.idle;
            StartCoroutine(WaitForSeconds(4, () =>
            {
                if (stats.state == EnemyAiState.pursuit&& stats.state != EnemyAiState.stun)
                {
                    stats.state = EnemyAiState.wander;
                }
            }));
        

    }

    public static IEnumerator WaitForSeconds(float duration, Action action = null)
    {
        yield return new WaitForSeconds(duration);
        action?.Invoke();
    }

    public void TakeDamage(int damage,int stun,float knockback,AllPlayerCharacter attacker)
    {
        if (stats.state == EnemyAiState.stun)
        {
            stats.hp -= damage;
            statusBars.UpdateSlider("hp", stats.hp);
        }
        else
        {

            stats.hp -= damage;
            statusBars.UpdateSlider("hp", stats.hp);

            stats.stun += stun;
            statusBars.UpdateSlider("stun", stats.stun);
        }


        if (!stats.superarmor)
        {
            stats.state = EnemyAiState.idle;
            animator.Play("BeAttack_enemy", 0);
            Knockback(knockback);
        }
        hptimer=Time.time+hpdisabletime;
        hpBase.SetActive(true);
        AudioSource.PlayClipAtPoint(hitSound, transform.position);

        if (!GameManager.Instance.IsthisCtrolingCharacter(attacker))
        {
            if (GameManager.Instance.GetPlayerGameObject(attacker).GetComponent<FriednlyAI>().target == this.gameObject)
            {
                target = GameManager.Instance.GetPlayerGameObject(attacker);
                nav.targetCharacter=attacker;
            }
        }
        else
        {
            target = GameManager.Instance.GetPlayerGameObject(attacker);
        }

        if (stats.stun >= maxStun)
        {
            stats.stun=maxStun;
            animator.Play("Stun");

            stats.state = EnemyAiState.stun;

        }



        if (stats.hp < 1)
        {

            nav.target = transform.position;

            stats.state = EnemyAiState.dead;

        }
    }

     void Knockback(float rate)
    {
        knockbackControl.Knockback(rate);

    }
    internal void SetStats(EnemyStats stats)
    {
        this.stats = new EnemyStats(stats);
    }

    public void SetToPursuit()
    {
        if (stats.state == EnemyAiState.idle)
        {

            stats.state = EnemyAiState.pursuit;

            if (Vector3.Distance(transform.position, target.transform.position) <= 2f)
            {
                stats.state = EnemyAiState.wander;
            }
        }
    }

    private void UpdateTargetSensor()
    {
        if (targetSensor.GetNearestDetection() != null)
        {
            if (GameManager.Instance.IsControlling)
            {
                target = targetSensor.GetNearestDetection();
                nav.targetCharacter = GameManager.Instance.controlingCharacter;
            }
        }

    }
    public void ActiveAlertGroup()
    {
        if (alertGroup[0] == null) { return; }

        foreach (var alert in alertGroup)
        {
            alert.SetTarget(target);
        }
    }
    public void SetTarget(GameObject @object)
    {
        target = @object;
    }
    public void GetSuperArmor(float t)
    {
        stats.superarmor=true;

        StartCoroutine(WaitForSeconds(t, () =>
        {
            stats.superarmor = false;
        }));
    }

    public bool islocked = false;
    [SerializeField] float hpdisabletime = 2f;
    float hptimer;

    void IDamageable.TakeDamage(int damage, float knockback, Transform attackform)
    {
        return;
    }
}
