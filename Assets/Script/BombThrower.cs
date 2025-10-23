using UnityEngine;
using static BossControl;

public class BombThrower : MonoBehaviour
{
    public GameObject bomb;
    public void ThrowBomb(BombPattern pattern,Transform target)
    {
        Vector3 start = transform.position;

        Vector3 control;
        Vector3 end;

        switch (pattern)
        {
            case BombPattern.Random:
                control = GetNewWanderPosition() + Vector3.up * 10;
                end = target.transform.position;
                break;


            case BombPattern.Line:
                // 在玩家前方一整條直線丟炸彈
                Vector3 forwardOffset = target.transform.forward * 5f;
                control = target.transform.position + forwardOffset + Vector3.up * 10;
                end = target.transform.position + forwardOffset;
                break;

            case BombPattern.Circle:
                // 以玩家為中心，固定半徑一圈
                float angle = UnityEngine.Random.Range(0f, 360f);
                Vector3 circlePos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 6f;
                control = target.transform.position + circlePos + Vector3.up * 10;
                end = target.transform.position + circlePos;
                break;

            default:
                control = GetNewWanderPosition() + Vector3.up * 10;
                end = target.transform.position;
                break;
        }

        GameObject b = Instantiate(bomb);
        b.GetComponent<BossProjectile>().SetPoint(start, control, end);
    }

    private Vector3 GetNewWanderPosition()
    {
        Vector3 wanderPosition;
        float angle = UnityEngine.Random.Range(0, 360);
        Vector3 offset = new Vector3(Mathf.Cos(angle) * 5, 0, Mathf.Sin(angle) * 5);
        wanderPosition = transform.position + offset;
        return wanderPosition;
    }
}
