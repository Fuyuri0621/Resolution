using UnityEngine;

using UnityEngine.AI;

public class NavToTarget : MonoBehaviour
{
   public GameObject marker;

    public Transform ChinLi;
    public Transform TanTong;
    public bool needFaceplayer;

    public Vector3 target;
    [SerializeField] bool allowMoving;
    public NavMeshAgent agent;
     Animator animator;

    Vector2 Velocity;
    Vector2 SmoothDeltaPosition;
    int movevector_xHash;
    int movevector_yHash;
   public AllPlayerCharacter targetCharacter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        movevector_xHash = Animator.StringToHash("movevector_x");
        movevector_yHash = Animator.StringToHash("movevector_y");
    }
    void Start()

    {
        target = transform.position;
        ChinLi= GameManager.Instance.GetPlayerGameObject(AllPlayerCharacter.ChinLi).transform;
        TanTong=GameManager.Instance.GetPlayerGameObject(AllPlayerCharacter.TanTong).transform;
        agent.updatePosition = true;
        agent.updateRotation = true;

    }

   /* private void OnAnimatorMove()
    {
        Vector3 rootposition = animator.rootPosition;
        rootposition.y = agent.nextPosition.y;
      //  transform.position = rootposition;
       // agent.nextPosition = rootposition;
    }*/
    void Update()
    {
        if (needFaceplayer) { FacePlayer(); }

        animator.SetFloat("MoveVelocity", agent.velocity.magnitude);
       


        MoveCheck();
        if (target!=null&& agent.enabled) 
        agent.SetDestination(target);

        if(marker!=null) 
        marker.transform.position = target;

    }
    private void MoveCheck()
    {
        if (allowMoving != agent.enabled) { agent.enabled = !agent.enabled; }
        else { return; }


    }
    public  void SetSpeed(float speed)
    {
        agent.speed = speed;
    }

    public float GetSpeed()
    {
        return agent.speed;
    }

    void FacePlayer()
    {

        switch (targetCharacter)
        {
            case AllPlayerCharacter.ChinLi:
                transform.LookAt(new Vector3 (ChinLi.transform.position.x,transform.position.y,ChinLi.position.z));
                break;
            case AllPlayerCharacter.TanTong:
                transform.LookAt(new Vector3(TanTong.transform.position.x, transform.position.y, TanTong.position.z));
                break;
        }

        Vector3 worldMoveDirection = agent.velocity.normalized;
        Vector3 localMoveDirection = transform.InverseTransformDirection(worldMoveDirection);
        localMoveDirection = localMoveDirection.normalized;
        animator.SetFloat(movevector_xHash, localMoveDirection.x);
        animator.SetFloat(movevector_yHash, localMoveDirection.z);

    }

    void ReleseHand()
    {
        if (animator.GetBool("holdWeapon"))
            animator.SetBool("holdWeapon", false);
        else animator.SetBool("holdWeapon", true);
    }
}