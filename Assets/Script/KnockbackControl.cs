using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class KnockbackControl : MonoBehaviour
{
    private Rigidbody rb; // 角色剛體
    Transform character;
   // public float knockbackForce = 10f; // 擊退的力量
    public float knockbackDuration = 0.5f; // 擊退的持續時間

    private float knockbackElapsedTime = 1f;
    public bool isKnockbackActive = false; // 是否在擊退狀態

    Vector3 knockbackStartPos;
    Vector3 knockbackTargetPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // 獲取 Rigidbody
        character = rb.GetComponent<Transform>();
    }

    public void Pushforward(float distance)
    {
        ApplyKnockback(transform.TransformDirection(Vector3.forward), distance);
    }

    public void PushToPlayer(float distance)
    {
        isKnockbackActive = true;
        knockbackElapsedTime = 0f;
        Transform target = GameManager.Instance.GetCtrolingCharacter().transform;
        Vector3 direction = transform.position - target.transform.position;
        direction = direction.normalized;

        knockbackStartPos = character.position;
        knockbackTargetPos = target.position + (direction * distance);
        
    }

    public void FlashToPlayer(float distance)
    {
        //   isKnockbackActive = true;
        knockbackElapsedTime = 0f;
        Transform target = GameManager.Instance.GetCtrolingCharacter().transform;
        Vector3 direction = transform.position - target.transform.position;
        direction = direction.normalized;

        knockbackStartPos = character.position;
        knockbackTargetPos = target.position + (direction * distance);
        character.position = knockbackTargetPos;
    }

    public void Knockback(float distance)
    {
        ApplyKnockback(transform.TransformDirection(Vector3.back), distance);
    }

    public void Knockback(float distance, Vector3 direction)
    {
        ApplyKnockback(transform.TransformDirection(direction), distance);
    }

    private void ApplyKnockback(Vector3 direction, float distance)
    {
        if (rb == null) return; 

        isKnockbackActive = true;
        knockbackElapsedTime = 0f;

        knockbackStartPos = character.position;
        knockbackTargetPos = character.position + (direction * distance);

        Debug.DrawRay(character.position + Vector3.up, direction, Color.green, distance);

        RaycastHit hit;
        int layerMask = LayerMask.GetMask("WALL", "enemy","Player");
        if (Physics.Raycast(character.position+Vector3.up, direction, out hit, distance, layerMask))
        {
            if (direction!=transform.TransformDirection(Vector3.up))
            {
                Debug.Log("WallDect" + new Vector3(hit.point.x, knockbackTargetPos.y, hit.point.z));
                knockbackTargetPos = new Vector3(hit.point.x, knockbackTargetPos.y, hit.point.z);
            }
        }


      //  Vector3 knockbackVelocity = direction.normalized * (knockbackForce * (distance / knockbackDuration));
      //  rb.linearVelocity = Vector3.zero; 
       // rb.AddForce(knockbackVelocity, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (isKnockbackActive)
        {
                  knockbackElapsedTime += Time.fixedDeltaTime;

                  if (knockbackElapsedTime >= knockbackDuration)
                  {
                      isKnockbackActive = false;
                  //    rb.linearVelocity = Vector3.zero; // 停止剛體運動
                  }
            knockbackElapsedTime += Time.fixedDeltaTime;
            character.position = Vector3.Lerp(knockbackStartPos, knockbackTargetPos, knockbackElapsedTime / knockbackDuration);

        }

    }
}
