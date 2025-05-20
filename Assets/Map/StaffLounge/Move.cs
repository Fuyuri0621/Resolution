using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Move : MonoBehaviour
{
    Rigidbody rb;
    GameObject Cam;


     Transform target; 
     float x, y;

    [SerializeField]
    [Header("移動速度")]
    int MoveSpeed=5;
    Vector2 MoveVector = Vector2.zero;

    [SerializeField]
    [Header("靈敏度")]
    public float Speed = 200;//靈敏度

    [SerializeField]
    [Header("當前攝影機與主角的距離")]
    public float distence=5;//當前攝影機與主角的距離

    [SerializeField]
    [Header("y軸上限視角")]
    [Range(-89, 89)]
    public float maxy = 89;
    [SerializeField]
    [Header("y軸下限視角")]
    [Range(-89, 89)]
    public float miny = -45;

    private float fy = 0;//計算y座標用
    private Quaternion rotationEuler;
    private Vector3 cameraPosition;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        Cursor.visible = false;//隱藏滑鼠
        Cursor.lockState = CursorLockMode.Locked;//把滑鼠鎖定到螢幕中間
        target= transform;
        Cam = GameObject.Find("Main Camera");
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        MoveVector.x = -Input.GetAxis("Horizontal");
        MoveVector.y = -Input.GetAxis("Vertical");

        if (MoveVector != new Vector2(0, 0))
        {



            transform.LookAt(new Vector3(Cam.transform.position.x, transform.position.y, Cam.transform.position.z));

            transform.Translate(new Vector3(MoveVector.x, 0, MoveVector.y) * Time.deltaTime * MoveSpeed);


        }



        //讀取滑鼠的X、Y軸移動訊息
        x += Input.GetAxis("Mouse X") * Speed * Time.deltaTime;
        fy = y - Input.GetAxis("Mouse Y") * Speed * Time.deltaTime;
        //限制y的角度
        if (!(fy <= miny || fy >= maxy))
        {
            y = fy;
        }
        //保護角度度數
        x %= 360;
        y %= 90;
        //運算攝影機座標、旋轉
        rotationEuler = Quaternion.Euler(y, x, 0);
        cameraPosition = rotationEuler * new Vector3(0, 0, -distence) + (target.position + new Vector3(0, 1, 0));

        //應用
        Cam.transform.rotation = rotationEuler;
        Cam.transform.position = cameraPosition;
    }
}
