using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static BossControl;

public class BossSpecialCol : MonoBehaviour
{
    [Header("演出起跳")]
    public Transform actPoint;
    public Transform controlPoint;  // 曲線中間控制點
    Vector3 startPoint;

    public float travelTime = 2f;  // 從 0~1 的時間
    [Range(0, 1), SerializeField] private float _duration = 0f;

    [Header("投擲控制")]
    List<Transform> p;

    public float attackCD=10;
    float attacktimer;
    BombThrower bombThrower;
    void Start()
    {
        attacktimer = Time.time+5;
        startPoint = transform.position;
        bombThrower =GetComponent<BombThrower>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_duration < 1f)
        {
            _duration += Time.deltaTime / travelTime;
            transform.position = CalculateBezierPoint(_duration, startPoint, controlPoint.position, actPoint.position);
        }
       if(attacktimer < Time.time)
        {
            attacktimer = Time.time+attackCD;

            StartCoroutine(Phase2Transition());

        }


    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // 二次貝茲曲線公式
        return Mathf.Pow(1 - t, 2) * p0 +
               2 * (1 - t) * t * p1 +
               Mathf.Pow(t, 2) * p2;
    }


    IEnumerator Phase2Transition()
    {
        BombPattern pattern = ((BombPattern)Random.Range(0, 3));

        float duration = 6f;
        float interval = 1f;
        float timer = 0f;

        while (timer < duration)
        {

            bombThrower.ThrowBomb(pattern, GameManager.Instance.GetPlayerGameObject(GameManager.Instance.controlingCharacter).transform);
            yield return new WaitForSeconds(interval);
            timer += interval;
        }



    }
}
