using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static CinemachineShakeManager;

public class ComboManager : MonoBehaviour,IHitStopHandler
{
    public Transform effectpos;
    public AudioClip swingSound;

    public WeaponConfig currentWeaponConfig;
    public float releaseTime;

    Animator animator;
    ComboInput comboInput;

   float releaseTimer;
   public bool isOnNeceTime;
    ComboConfig currentComboConfig;

    int lightAttackIdx = 0;
    int heavyAttackIdx = 0;

    [SerializeField] const float animationFadeTime = 0.1f;

    public AllPlayerCharacter thisCharacter;

   public int currentDamageRate;
   public int currentStunRate;
    public float currentknockbackrate;
    ShakeStrength currentshakeStrength;
    public Vector3 size;
    public float zoffset;
    Playercontrol1 playercontrol;
    KnockbackControl knockbackControl;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (Time.time > releaseTimer&&(lightAttackIdx!=0||heavyAttackIdx!=0))
        {
            Debug.Log("stopcombo");
            StopCombo();
        }
    }
    void Init()
    {
        playercontrol =GetComponent<Playercontrol1>();
        animator = GetComponent<Animator>();
        comboInput = GetComponent<ComboInput>();
        thisCharacter = GetComponent<Playercontrol1>().thisCharacter;
        knockbackControl = GetComponent<KnockbackControl>();
    }



    public void OnLightAttack(InputValue value)
    {
        if (isOnNeceTime) { return; }
        if (GameManager.Instance.controlingCharacter != thisCharacter) { return; }
        NormalAttack(true);
    }

    public void OnHeavyAttack(InputValue value)
    {
        if (GameManager.Instance.controlingCharacter != thisCharacter) { return; }
        if (isOnNeceTime) { return; }

        NormalAttack(false);
    }

    bool haveHole;
    IEnumerator PlayCombo(ComboConfig comboConfig)
    {
        isOnNeceTime = true;
        releaseTimer = Time.time + releaseTime;
        currentComboConfig = comboConfig;


        currentDamageRate=comboConfig.damagerate;
        currentStunRate=comboConfig.stunrate;
        currentknockbackrate = comboConfig.knockbackrate;

        zoffset = comboConfig.attackzoffset;
        size.x = comboConfig.attackwidth;
        size.z = comboConfig.attacklength;
        haveHole = comboConfig.haveHold;
        currentshakeStrength = comboConfig.shakeStrength;

        animator.CrossFade(comboConfig.animatorStateName, animationFadeTime);

        if (comboConfig.effectName != "") { EffectManager.Instance.SpawnEffectByName(comboConfig.effectName, effectpos); }
        

        float passtime = 0f;

        if (haveHole) //蓄力攻擊
        {
            float damagerate=0;
            Debug.Log("開始按住");
            animator.SetBool("isHold", true);
            while (true)
            {
                if (Mouse.current.rightButton.wasReleasedThisFrame|| passtime>= comboConfig.releaseTime)
                {
                    damagerate = passtime / comboConfig.releaseTime;
                    if (damagerate < 0.4f) damagerate = 0.4f;
                    break;
                }
                passtime += Time.deltaTime;

                yield return null;
            }
            currentDamageRate = (int)(currentDamageRate*damagerate);
            Debug.Log("結束按住");
            animator.SetBool("isHold", false);
            isOnNeceTime = false;
        }
        else//非蓄力攻擊
        {
            while (true)
            {
                if (passtime >= comboConfig.releaseTime)
                {
                    break;
                }
                passtime += Time.deltaTime;

                yield return new WaitForSeconds(Time.deltaTime);
            }

            isOnNeceTime = false;
        }
        //BacktoMoveFormCombo();
        yield break;
    }

    public void NormalAttack(bool isLight)
    {
        if (!GameManager.Instance.IsControlling) {return; }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge")) { return; }

        playercontrol.RotateToInputVector();
        playercontrol.RotateToInputVector();
        List<ComboConfig> configs = isLight ? currentWeaponConfig.lightComboConfig : currentWeaponConfig.heavyComboConfig;
        int comboIdx = isLight ? lightAttackIdx : heavyAttackIdx;
        StartCoroutine(PlayCombo(configs[comboIdx]));




        if (comboIdx >= configs.Count - 1)
        {
            comboIdx = 0;
        }
        else 
        {
            comboIdx++;
        }

        if(isLight)
        {
            lightAttackIdx = comboIdx;
            if (thisCharacter == AllPlayerCharacter.TanTong)
            { heavyAttackIdx = 3 * lightAttackIdx - 2;if (heavyAttackIdx == 7) { heavyAttackIdx = 0; } }

            else if(heavyAttackIdx!=0)
            {
                lightAttackIdx = 1; heavyAttackIdx = 0;
            }
        }
        else //hvyatk
        {
            if (thisCharacter == AllPlayerCharacter.ChinLi &&lightAttackIdx != 0)
            { comboIdx = 1; lightAttackIdx = 0; }
            else if(lightAttackIdx != 0){
                lightAttackIdx = 0;
            };
            


            heavyAttackIdx = comboIdx;
        }
        Debug.Log("comboIdx" + comboIdx);
        AudioManager.Instance.PlaySFX(swingSound, transform.position);
    }

    void StopCombo()
    {
        lightAttackIdx= 0;
        heavyAttackIdx = 0;
    }


    void BacktoMoveFormCombo()
    {
       
        if (animator.GetFloat("movevector_x") != 0 || animator.GetFloat("movevector_x") != 0)
        { animator.CrossFade(animator.GetFloat("isbattle") > 0.5 ? "battlemove" : "move 0", 0.1f); playercontrol.RotateToInputVector(); playercontrol.RotateToInputVector(); }
    }
     void AttackOnBox()
    {

        Vector3 localOffset = new Vector3(0, 1, zoffset + size.z / 2f);
        Vector3 pos = transform.TransformPoint(localOffset);


        Collider[] colliders = Physics.OverlapBox(pos, size,transform.rotation,LayerMask.GetMask("enemy"));
        for (int i = 0; i < colliders.Length; i++)
        {
           IDamageable traget = colliders[i].GetComponent<IDamageable>();
            if (traget != null)
            {

                traget.TakeDamage(currentDamageRate, currentStunRate, currentknockbackrate, thisCharacter);
               
                bool needback=false;
                foreach (Collider e in colliders)
                {
                    if (Vector3.Distance(transform.position, e.transform.position) < 1f) { needback = true; }
                }
                if(needback)knockbackControl.Knockback(0.3f);
                StartCoroutine(GameManager.Instance.PlayerHitStop(thisCharacter, GameManager.Instance.hitstopduration));
                if (GameManager.Instance.IsthisCtrolingCharacter(thisCharacter))
                { CinemachineShakeManager.Instance.Shake(currentshakeStrength); }
            }
        }

        AttackPreview preview = GetComponent<AttackPreview>();
        if (preview != null)
        {
            preview.ShowBoxFX(pos, size, transform.rotation);
        }
    }

    public void OnAttackEffect(string effectname)
    {
        EffectManager.Instance.SpawnEffectByName(effectname, effectpos);
    }

    public IEnumerator Cantattacksec(float t)
    {
        isOnNeceTime = true;
        float passtime = 0f;
        while (true)
        {
            if (passtime >= t)
            {
                break;
            }
            passtime += Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime);
        }
        isOnNeceTime = false;
    }
    private void OnDrawGizmos()
    {

        Vector3 pos = transform.position + new Vector3(0, 1, (zoffset+size.z/2f));
        Gizmos.DrawWireCube(pos,size);


    }

    public void OnHitStop(bool pause)
    {
        if (animator != null) animator.speed = pause ? 0f : 1f;
    }
}
