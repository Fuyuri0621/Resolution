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
    [Header("���ʳt��")]
    int MoveSpeed=5;
    Vector2 MoveVector = Vector2.zero;

    [SerializeField]
    [Header("�F�ӫ�")]
    public float Speed = 200;//�F�ӫ�

    [SerializeField]
    [Header("��e��v���P�D�����Z��")]
    public float distence=5;//��e��v���P�D�����Z��

    [SerializeField]
    [Header("y�b�W������")]
    [Range(-89, 89)]
    public float maxy = 89;
    [SerializeField]
    [Header("y�b�U������")]
    [Range(-89, 89)]
    public float miny = -45;

    private float fy = 0;//�p��y�y�Х�
    private Quaternion rotationEuler;
    private Vector3 cameraPosition;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        Cursor.visible = false;//���÷ƹ�
        Cursor.lockState = CursorLockMode.Locked;//��ƹ���w��ù�����
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



        //Ū���ƹ���X�BY�b���ʰT��
        x += Input.GetAxis("Mouse X") * Speed * Time.deltaTime;
        fy = y - Input.GetAxis("Mouse Y") * Speed * Time.deltaTime;
        //����y������
        if (!(fy <= miny || fy >= maxy))
        {
            y = fy;
        }
        //�O�@���׫׼�
        x %= 360;
        y %= 90;
        //�B����v���y�СB����
        rotationEuler = Quaternion.Euler(y, x, 0);
        cameraPosition = rotationEuler * new Vector3(0, 0, -distence) + (target.position + new Vector3(0, 1, 0));

        //����
        Cam.transform.rotation = rotationEuler;
        Cam.transform.position = cameraPosition;
    }
}
