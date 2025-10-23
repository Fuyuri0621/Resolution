using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anifollow : MonoBehaviour
{
    [HideInInspector] public bool moving = false;
    public bool isfollowing = true;


    [SerializeField] public Transform followTarget;

    Animator animator;
    int postureHash;
    [SerializeField] float followDistance = 3;
    [SerializeField] float stopDistance = 1;
    [SerializeField] float chaceDistance = 3;
    [SerializeField] float speed = 2;
    // Start is called before the first frame update

    Rigidbody rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        postureHash = Animator.StringToHash("PlayerPosture");

        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        

        



            if (isfollowing)
            {

              
                if (stopDistance != 1.5f || speed != 3f)
                {
                    stopDistance = 1.5f;
                    chaceDistance = 3f;
                    speed = 3f;
                }

                NewMethod();
            }
        
        
    }

    private void NewMethod()
    {
        if (Vector3.Distance(transform.position, followTarget.position) >= followDistance)
        {
            moving = true;
            followDistance = stopDistance;
            transform.LookAt(new Vector3(followTarget.position.x, transform.position.y, followTarget.position.z));

            animator.SetFloat(postureHash, speed, 0.1f, Time.deltaTime);
             transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
        }
        else
        {
            moving = false;
            followDistance = chaceDistance;
            animator.SetFloat(postureHash, 0f, 0.1f, Time.deltaTime);
            //   animator.SetBool("ismove", false);
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
