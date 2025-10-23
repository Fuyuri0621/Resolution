using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class BossProjectile : MonoBehaviour
{
    [Header("貝茲曲線控制點 (由Boss設定)")]
    public Vector3 startPoint;
    public Vector3 controlPoint;   // 曲線中間控制點
    public Vector3 endPoint;

    [Header("飛行控制")]
    public float travelTime = 2f;  // 從 0~1 的時間
   [Range(0,1),SerializeField] private float _duration = 0f;  // 0~1 進度

    [Header("效果設定")]
    public float effectRadius = 5f;
    public float slowAmount = 0.8f;
    public float effectDuration = 5f;
    public float explosionDelay = 8f;
    public int explosionDmg = 30;

    public GameObject explosionEffect;

 //   private bool effectActive = false;
    Collider[] players;

    private List<Animator> playerAnimator = new List<Animator>();


    void Start()
    {
        GetComponent<SphereCollider>().radius = effectRadius;
        StartCoroutine(ExplosionCountdown(explosionDelay));
    }

    void Update()
    {
        // 依照 _duration 移動
        if (_duration < 1f)
        {
            _duration += Time.deltaTime / travelTime;
            transform.position = CalculateBezierPoint(_duration, startPoint, controlPoint, endPoint);
        }


        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(ExplosionCountdown(explosionDelay));
            StartCoroutine(ExplosionCountdown(1f));
            Debug.Log(other.GetComponent<Animator>().speed);
            other.GetComponent<Animator>().speed = slowAmount;
            Debug.Log(other.GetComponent<Animator>().speed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Animator>().speed = 1f;
        }
    }
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // 二次貝茲曲線公式
        return Mathf.Pow(1 - t, 2) * p0 +
               2 * (1 - t) * t * p1 +
               Mathf.Pow(t, 2) * p2;
    }

   

    private IEnumerator ExplosionCountdown(float time)
    {
        yield return new WaitForSeconds(time);
        Explode();
    }

    private void Explode()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        players = Physics.OverlapSphere(transform.position, effectRadius, LayerMask.GetMask("PLAYER"));
        foreach(Collider collider in players)
        {
            collider.GetComponent<Animator>().speed = 1f;
            IDamageable traget = collider.GetComponent<IDamageable>();
            if (traget != null) { traget.TakeDamage(explosionDmg, 2f, transform,CinemachineShakeManager.ShakeStrength.MID); }
        }
        Destroy(gameObject);
    }

    public void SetPoint(Vector3 start,Vector3 con,Vector3 end)
    {
        startPoint = start;
        endPoint = end;
        controlPoint = con;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(transform.position,effectRadius);
    }
}


