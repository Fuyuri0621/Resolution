using Micosmo.SensorToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;



public enum States
{
    idle,
    patrol,
    attack,
    pursuit,
    dead,

}
public class DetectPlayer1 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]  DetectPlayer[] alertGroup;
    public AudioClip hitSound;
    [SerializeField] bool canBattle = true;
    [SerializeField] GameObject failPanel;
    [SerializeField] Transform[] PatrolPath;
    [SerializeField] int tragetPatrolPath = 0;
    [SerializeField] float changePatrolCD = 5;

    [SerializeField] int HP = 100;

    [SerializeField] Slider hpSlider;

    [SerializeField] Slider suspiciousSlider;
    [SerializeField] Slider alertnessSlider;

    [SerializeField] GameObject backCanvas;
    [SerializeField] GameObject sliderCanvas;


    [SerializeField] Animator animator;
    [SerializeField] Transform backattackpos;

    [SerializeField] Sensor targetSensor;
    [SerializeField] Sensor BackSensor;
    [SerializeField] int suspiciousRate = 5;
    [SerializeField] float suspicious = 0f;
    [SerializeField] float alertness = 0;
    [SerializeField] float releaseTime = 5;

    float attackCD = 0;
 //   [SerializeField] bool IsAttacking = false;


    [SerializeField] NavToTarget nav;
    [SerializeField] RagDollControl dollControl;
    Vector3 lastSuspiciousLocation;
    GameObject target;
    bool isdead =false;
    Collider col;

    [SerializeField] State state = new State();

    private void Awake()
    {
        col = GetComponent<Collider>();
    }
    void Start()
    {
        state = State.patrol;
    }

    // Update is called once per frame
    void Update()
    {

        if (BackSensor.GetNearestDetection() != null)
        {
            if (GameManager.Instance.IsControlling)
            {
                backCanvas.SetActive(true);// BackSensor.GetNearestDetection().GetComponent<Playercontrol1>().AttackCd = 1; }
            }


        }
        else backCanvas.SetActive(false);

        if (targetSensor.GetNearestDetection() != null)
        {
            if (GameManager.Instance.IsControlling)
            { target = targetSensor.GetNearestDetection(); }
        }
        else target = null;

        switch (state)
        {
            case State.idle:
                //µo§b
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
                //¦º¤`
                if (!isdead) {
                    GetComponent<Collider>().enabled = false;
                    animator.Play("BeAttackdead");
                    nav.target = transform.position;
                    sliderCanvas.SetActive(false);
                    backCanvas.SetActive(false);
                    BackSensor.enabled = false;
                    targetSensor.enabled = false;
                    isdead = true;

               //     GameManager.Instance.RemovetBattleEmemies(this);
                    //GetComponent<NavMeshAgent>().enabled = false;


                    // dollControl.RagDollON();
                }
                break;

        }

        UpdateSlider(hpSlider, HP);
        UpdateSlider(suspiciousSlider, (int)suspicious);
        UpdateSlider(alertnessSlider, (int)alertness);
    }

    private void AttackMode()
    {
        nav.target = transform.position;
        if (attackCD == 0)
        {
           // animator.Play("Attack_enemy", 0);

            attackCD = 3;
        }

        if (attackCD <= 1)
        {
            state = State.pursuit;
        }
        if (attackCD > 0) { attackCD -= Time.deltaTime; }
    }
    private void PursuitMode()
    {
     
 //       if (releaseTime==0) { state = State.patrol; GameManager.Instance.RemovetBattleEmemies(this); }

        if (target != null)
        {
            lastSuspiciousLocation = target.transform.position;
            nav.target = target.transform.position;
            if (releaseTime != 3)releaseTime = 3;
        }
        else
        {
            releaseTime -= Time.deltaTime;
            if (releaseTime < 0)
                releaseTime = 0;
        }

        if (target != null && attackCD == 0 && Vector3.Distance(transform.position, target.transform.position) < 1.5)
        {

            state = State.attack;
        }

        if (attackCD < 0)
        {
            attackCD = 0;
        }
        if (attackCD > 0) { attackCD -= Time.deltaTime; }
    }
    private void PatrolMode()
    {

        if (target != null && GameManager.Instance.IsControlling)
        {
            if (canBattle)
            {
                suspicious = 100;
                alertness = 100;
                lastSuspiciousLocation = target.transform.position;
                ActiveAlearnGroup();
                state = State.pursuit;
            }

            AddSuspicious();


            lastSuspiciousLocation = target.transform.position;
            releaseTime = 3;

        }
        else
        {
            if (releaseTime > 0)
                releaseTime -= Time.deltaTime;
            if (releaseTime < 0)
                releaseTime = 0;


            if (releaseTime == 0)
            {
                ReduceAlertness();


                if (releaseTime > 0)
                    releaseTime -= Time.deltaTime;
                if (releaseTime < 0)
                    releaseTime = 0;
            }

            if (alertness == 0 && releaseTime == 0)
            {
                ReduceSuspicious();


                if (Vector3.Distance(PatrolPath[tragetPatrolPath].position, transform.position) < 1.3f)
                {
                    changePatrolCD -= Time.deltaTime;
                    if (changePatrolCD < 0)
                    {
                        tragetPatrolPath++;
                        if (tragetPatrolPath > PatrolPath.Length - 1) tragetPatrolPath = 0;
                        changePatrolCD = 5;

                    }

                }
                nav.target = PatrolPath[tragetPatrolPath].position;
            }
        }



    }

    private void AddSuspicious()
    {

        if (suspicious == 100)
        {
            nav.target = lastSuspiciousLocation;

            AddAlertness();
            return;
        }


        suspicious += (Time.deltaTime * suspiciousRate);

        if (suspicious > 100)
            suspicious = 100;

    }

    private void ReduceSuspicious()
    {
        if (suspicious == 0f)
            return;

        suspicious -= Time.deltaTime * 60;
        if (suspicious < 0) suspicious = 0;
    }

    private void AddAlertness()
    {

        if (alertness == 100)
        {

            if(!canBattle) {
                failPanel.SetActive(true);
                GameManager.Instance.PauseGame();
            }
            else
            {
   //             GameManager.Instance.AddtBattleEmemies(this);
            }
            state = State.pursuit;
            return;
        }


        alertness += (Time.deltaTime * suspiciousRate);

        if (alertness > 100)
            alertness = 100;

    }

    private void ReduceAlertness()
    {
        if (alertness == 0f)
            return;

        alertness -= Time.deltaTime * 60;
        if (alertness < 0) alertness = 0;
    }

    public void UpdateSlider(Slider targetSlider, int current)
    {

        targetSlider.value = current;
    }

    public void BackAttack()
    {

        if (backCanvas.activeInHierarchy == true) 
        { animator.Play("BeBackAttack", 0);
            nav.target = transform.position;
            sliderCanvas.SetActive(false);
            backCanvas.SetActive(false);
            if (BackSensor.enabled)
            {
                BackSensor.GetNearestDetection().gameObject.transform.position = backattackpos.position;
                BackSensor.GetNearestDetection().gameObject.transform.rotation = backattackpos.rotation;
                BackSensor.GetNearestDetection().gameObject.GetComponent<Animator>().Play("BackAttack", 0);


                BackSensor.enabled = false;
            }
            state = State.dead; }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "playerweapon")
        {
            Debug.Log("hit");
            Hited();
            Vector3 pos = col.ClosestPointOnBounds(other.transform.position);

            EffectManager.Instance.SpawnHitEffect(pos);
        }
    }
    public void Hited()
    {
            animator.Play("BeAttack_enemy", 0);
        AudioSource.PlayClipAtPoint(hitSound,transform.position);

        HP -= 10;
        if(HP <= 0) { nav.target = transform.position;
            sliderCanvas.SetActive(false);
            backCanvas.SetActive(false); state = State.dead; }
    }

    public void ActiveAlearnGroup()
    {
        if(alertGroup == null) return;

   /*     for (int i = 0; i < alertGroup.Length; i++)
        {
            alertGroup[i].target = target;
            alertGroup[i].lastSuspiciousLocation = target.transform.position;
            alertGroup[i].nav.target = target.transform.position;
            alertGroup[i].suspicious = 100;
            alertGroup[i].alertness = 100;
            state = State.pursuit;
        }*/
    }
}
