using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Micosmo.SensorToolkit;
using TMPro;
using Unity.VisualScripting;
using static BackpackLocalData;
using UnityEngine.InputSystem;

public enum FriednlyAIState
{
    idle,
    pursuit,
    wander,
    attack,
    dead
}
public class FriednlyAI : MonoBehaviour
{
    AllPlayerCharacter character;
    public FriednlyAIState stats;
    ComboManager comboManager;
    StatusBar statusBars;
    [SerializeField] private Animator animator;
    private follow follow;
    [SerializeField] private Micosmo.SensorToolkit.Sensor targetSensor;
    private Collider col;
    [SerializeField] private AudioClip hitSound;
  //  [SerializeField] float toWanderDistance = 2;
    [SerializeField] float wanderDistance = 2f;// 隨機徘徊的最大距離
    [SerializeField] float attackRange = 1.2f;
 //  [SerializeField] float outwanderDistance = 5f;
    public GameObject target;


    float attackCD = 1;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();
        follow = GetComponent<follow>();
        character = GetComponent<Playercontrol1>().thisCharacter;
        comboManager = GetComponent<ComboManager>();

    }
    private void Start()
    {


    }

    private void Update()
    {

        if(GameManager.Instance.controlingCharacter== character)
        {
            this.enabled = false;
        }

        UpdateTargetSensor();

        switch (stats)
        {
            case FriednlyAIState.idle:
                // Idle behavior
                break;
            case FriednlyAIState.pursuit:
                PursuitMode();
                break;
            case FriednlyAIState.wander:

                break;
            case FriednlyAIState.attack:
                AttackMode();
                break;
            case FriednlyAIState.dead:

                break;
        }



    }


    private void PursuitMode()

    {
        if (follow.GetSpeed() != 1.5f)
        {
            follow.SetSpeed(1.5f);
        }

        if (target != null)
        {

            if (Vector3.Distance(transform.position, target.transform.position) <= 2f)
            {
                stats = FriednlyAIState.attack;
            }
            
       //     nav.target = pursuitPosition;
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
        if (follow.GetSpeed() != 1f)
        {
            follow.SetSpeed(1f);
        }

        if (attackCD > 0) { attackCD = Math.Max(0, attackCD - Time.deltaTime); }

        if (target != null && attackCD == 0)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
            {
                Attack();
            }
        }


    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemyweapon"))
        {

            Vector3 pos = col.ClosestPointOnBounds(other.transform.position);
            EffectManager.Instance.SpawnHitEffect(pos);
        }
    }

    private void Attack()
    {
        attackCD = 3f;

        //animator.Play("Attack_player");
        stats = FriednlyAIState.pursuit;
        transform.LookAt(target.transform.position);
        comboManager.NormalAttack(true);
        StartCoroutine(WaitForSeconds(0.6f, () =>
        {
            transform.LookAt(target.transform.position);
            comboManager.NormalAttack(true);
        }));
        StartCoroutine(WaitForSeconds(1.2f, () =>
        {
            transform.LookAt(target.transform.position);
            comboManager.NormalAttack(true);
        }));


    }

    public static IEnumerator WaitForSeconds(float duration, Action action = null)
    {
        yield return new WaitForSeconds(duration);
        action?.Invoke();
    }



    private void UpdateTargetSensor()
    {
        if (targetSensor.GetNearestDetection() != null)
        {
            if (GameManager.Instance.IsControlling)
            {
                target = targetSensor.GetNearestDetection();
                follow.followTarget = target.transform;
            }
        }else follow.followTarget = null;

    }

    public void SetTarget(GameObject @object)
    {
        target = @object;
    }


}
