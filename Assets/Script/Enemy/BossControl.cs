using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Micosmo.SensorToolkit;
using Unity.Cinemachine;

public class BossControl : MonoBehaviour,IDamageable
{
    public EnemyStats stats;
    public int maxStun=100;
    public int nextstagehp;
    [SerializeField] bool nextStage;
    public int stunTimeRecoverRate=1;
    private float stunTimer;
    public float attackRange=0.8f;
    public float attackCoolDown = 2f;
    [SerializeField] float rangeToShoot = 30f;
    StatusBar statusBars;
    [SerializeField] private Transform[] PatrolPath;
    private int tragetPatrolPath = 0;
    private float changePatrolCD = 5f;
    float shoottime;

    [SerializeField] GameObject sliderCanvas;
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
    [SerializeField] GameObject nextStageCam;
   public GameObject BossinfoPanel;
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
    }

    private void Update()
    {

        if (GameManager.Instance.IsGameRunning)
        {

            switch (stats.state)
            {
                case EnemyAiState.idle:
                    IdleMode();
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
        }
        if(target!=null)
        nav.targetCharacter= target.GetComponent<Playercontrol1>().thisCharacter;
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

           nav.needFaceplayer = true;
            GameManager.Instance.AddtBattleEmemies(this.gameObject);

            animator.SetBool("isbattle",true);
            stats.state = EnemyAiState.pursuit;


        }
       
    }
    private void PursuitMode()

    {
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

            float distance = Vector3.Distance(transform.position, pursuitPosition);

            if (distance <= 0.3f)
            {
                stats.state = EnemyAiState.wander;
            }
            
            if (distance > rangeToShoot&&shoottime<Time.time)
            {
                shoottime = Time.time + 6;//6=shootcd;
                ShootAttack();
                
            }

            nav.target = pursuitPosition;
        }
    }

    private void WanderMode()
    {
        if (nav.GetSpeed() != 1f)
        {
            nav.SetSpeed(1f);
        }

        if (attackCD > 0) { attackCD = Math.Max(0, attackCD - Time.deltaTime); }

        if (target != null && attackCD == 0)
        {
           // if (GameManager.Instance.IsthisCtrolingCharacter( target.GetComponent<Playercontrol1>().thisCharacter))
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
         //   nav.target = pursuitPosition;
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
        if (nav.GetSpeed()!=2.5f)
        {
            nav.SetSpeed(2.5f);
        }

        Vector3 direction = transform.position - target.transform.position;
        direction = direction.normalized;
        Vector3 pursuitPosition = target.transform.position + direction * attackRange;

        nav.target= pursuitPosition;

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < attackRange+0.5f)
        {
            Attack();
        }

        if (distance > rangeToShoot)
        {
            shoottime = Time.time + 6;//10=shootcd;
            ShootAttack();

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
            nav.needFaceplayer = true;
            stats.state= EnemyAiState.pursuit;
        }

    }


    private void HandleDeath()
    {
        BossinfoPanel.SetActive(false);
        nav.needFaceplayer = false;
        col.enabled = false;
        animator.Play("BeAttackdead");
        nav.target = transform.position;
        sliderCanvas.SetActive(false); 
        targetSensor.enabled = false;
        Destroy(gameObject, 5);
    }

    private void UpdatePatrolPath()
    {
        if(PatrolPath.Length == 0) { return; }
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

    void ShootAttack()
    {
        animator.Play(RandomRangeAttack());
        attackCD =attackCoolDown;
        stats.state = EnemyAiState.idle;
        StartCoroutine(WaitForSeconds(4, () =>
        {
            if (stats.state == EnemyAiState.pursuit && stats.state != EnemyAiState.stun)
            {
                stats.state = EnemyAiState.wander;
            }
        }));
    }

   [SerializeField] List<string> attacklist;
    [SerializeField] List<string> rangeAttacklist;
    private void Attack()
    {
            animator.Play(RandomAttack());
            attackCD = attackCoolDown;
            stats.state = EnemyAiState.idle;
            StartCoroutine(WaitForSeconds(4, () =>
            {
                if (stats.state == EnemyAiState.pursuit&& stats.state != EnemyAiState.stun)
                {
                    stats.state = EnemyAiState.wander;
                }
            }));
        

    }

    string RandomAttack()
    {
        if (attacklist.Count == 0) return null;


          return  attacklist[UnityEngine.Random.Range(0, attacklist.Count)];

    }

    string RandomRangeAttack()
    {
        if (rangeAttacklist.Count == 0) return null;


        return rangeAttacklist[UnityEngine.Random.Range(0, attacklist.Count)];

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
            if(stats.hp<= nextstagehp&&!nextStage)
            {
                NextStage();
            }
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
        AudioSource.PlayClipAtPoint(hitSound, transform.position);

        if (!GameManager.Instance.IsthisCtrolingCharacter(attacker))
        {
            if (GameManager.Instance.GetPlayerGameObject(attacker).GetComponent<FriednlyAI>().target == this.gameObject)
            {
                target = GameManager.Instance.GetPlayerGameObject(attacker);
                nav.targetCharacter = attacker;
            }
        }
        else
        {
            target = GameManager.Instance.GetPlayerGameObject(attacker);
            nav.targetCharacter = attacker;
        }

        if (stats.stun >= maxStun)
        {
            stats.stun=maxStun;
            animator.Play("Stun");
            nav.needFaceplayer =false;
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

    public void SetTarget(GameObject @object)
    {
        target = @object;
    }
    public void GetSuperArmor(float t)
    {
        stats.superarmor = true;

        StartCoroutine(WaitForSeconds(t, () =>
        {
            stats.superarmor = false;
        }));
    }

    public void NextStage()
    {
        attacklist.Add("BossAttackC") ;
        attacklist.Add("BossAttackC");
        GameManager.Instance.PlayingAni();
       GameObject cam = Instantiate(nextStageCam);
        cam.GetComponent<CinemachineCamera>().Follow = transform;
        nextStage=true;
        
    }
    void IDamageable.TakeDamage(int damage, float knockback, Transform attackform)
    {
        return;
    }
}
