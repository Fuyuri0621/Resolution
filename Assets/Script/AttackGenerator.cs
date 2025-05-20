using System.Drawing;
using UnityEngine;



public class AttackGenerator : MonoBehaviour
{
    public int currentDamageRate;
    public int currentStunRate;
    public float currentknockbackrate;
    public Vector3 size;
    public float zoffset;
   [SerializeField] AttatkData attackData;
    void Start()
    {
        attackData = Resources.Load<AttatkData>("Combo/AttatkData");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AttackOnBox(AttackID iD)
    {
       Attackinfo attackInfo = attackData.info.Find(x=>x.attackID == iD);
        Debug.Log(attackInfo.attackID);
        currentDamageRate=attackInfo.damageRate;
        currentknockbackrate=attackInfo.knockbackRate;
        currentStunRate=attackInfo.stunRate;
        zoffset=attackInfo.zoffset;
        size = attackInfo.size;

        Vector3 pos = transform.position + transform.forward * (zoffset + size.z / 2f);
        Collider[] colliders = Physics.OverlapBox(pos, size,transform.rotation);
        for (int i = 0; i < colliders.Length; i++)
        {
            IDamageable traget = colliders[i].GetComponent<IDamageable>();
            if (traget != null) { traget.TakeDamage(currentDamageRate, currentknockbackrate, transform); }
        }
    }
    public void OnAttackEffect(string effectname)
    {
        EffectManager.Instance.SpawnEffectByName(effectname, transform);
    }
    private void OnDrawGizmos()
    {

        Vector3 pos = transform.position + new Vector3(0, 1, (zoffset + size.z / 2f));
        Gizmos.DrawWireCube(pos, size);
    }
}
