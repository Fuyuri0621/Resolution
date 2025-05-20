using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    public bool isfollowing=true;
    Playercontrol1 playercontrol1;

    [SerializeField] public Transform followTarget;

    Animator animator;
    int postureHash;
    [SerializeField] float followDistance=3;
    [SerializeField] float stopDistance = 1;
    [SerializeField] float chaceDistance = 3;
    [SerializeField] float speed=2;
    // Start is called before the first frame update
    void Start()
    {
        postureHash = Animator.StringToHash("PlayerPosture");
        animator = GetComponent<Animator>();
        playercontrol1= GetComponent<Playercontrol1>();
    }

    // Update is called once per frame
    void Update()
    {
        if(followTarget == null) {followTarget = GameManager.Instance.GetCtrolingCharacter().transform; }

        if (GameManager.Instance.controlingCharacter!=playercontrol1.thisCharacter&& isfollowing)
        {

            if (GameManager.Instance.isBattling)
            {
                stopDistance = 1.2f;
                chaceDistance = 1.4f;
            }
            else if (stopDistance!=1.5f||speed!=3f)
            {
                stopDistance = 1.5f;
                chaceDistance = 3f;
                speed = 3f;
            }


            if (Vector3.Distance(transform.position, followTarget.position) >= followDistance)
            {
                followDistance = stopDistance;
                transform.LookAt(new Vector3(followTarget.position.x,transform.position.y,followTarget.position.z));
                animator.SetFloat("movevector_x", 0);
                animator.SetFloat("movevector_y", 1);
                animator.SetBool("ismove", true);
                animator.SetFloat(postureHash, speed, 0.1f, Time.deltaTime);
                // transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
            }
            else
            {
                followDistance = chaceDistance;
                animator.SetFloat(postureHash, 0f, 0.1f, Time.deltaTime);
                animator.SetFloat("movevector_x", 0);
                animator.SetFloat("movevector_y", 0);
                animator.SetBool("ismove", false);
            }
        }
    }

    public float GetSpeed()
    {
        return speed;   
    }
    public void SetSpeed(float s)
    {
        if (s > 3f) { s = 3f; }
        speed = s;
        
    }
}
