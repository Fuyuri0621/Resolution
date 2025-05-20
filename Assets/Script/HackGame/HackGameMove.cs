using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class HackGameMove : MonoBehaviour
{
    // Start is called before the first frame update
    StageEventManager stageEventManager;
   [SerializeField] Vector3 destination;
    public float speed = 1f;
    public int hp = 3;
   [SerializeField] LayerMask layerMask;

    private void Awake()
    {
        stageEventManager = gameObject.GetComponentInParent<StageEventManager>();
    }
    void Start()
    {
        destination=transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != destination)
        {

            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }
        else
            transform.position = destination;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null) { if (other.tag == "HackBarrier") { Hit(); Debug.Log("HIT"); } }
    }

    void Hit()
    {
        if (hp > 0) { hp -= 1; }
        if (hp == 0) { Dead(); }
    }
    void Dead()
    {
        
        stageEventManager.Losestage();
        destination = transform.position;
        hp = 3;
    }
    public void OnLeft(InputValue value)
    {
        if (value.isPressed && transform.position == destination)
        {
            MoveTo(Vector3.right);
            
        }
    }
    public void OnRight(InputValue value)
    {
        if (value.isPressed && transform.position == destination)
        {
            MoveTo(Vector3.left);
        }
    }
    public void OnUp(InputValue value)
    {
        if (value.isPressed && transform.position == destination)
        {
            MoveTo(Vector3.back);

        }
    }

    public void OnDown(InputValue value)
    {
        if (value.isPressed && transform.position == destination)
        {
            MoveTo(Vector3.forward);

        }
    }

    private void MoveTo(Vector3 vector)
    {

        RaycastHit hit;
        Ray ray = new Ray(transform.position, vector*-1);

        if (Physics.Raycast(ray, out hit, 50, layerMask))
        {
            destination = hit.point + vector;
        }
    }
}
