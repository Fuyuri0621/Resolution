using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class cameracontrol : MonoBehaviour
{
    Vector2 MoveVector;
    Quaternion Rotation;
    float x;
    float y;
    public float mouseSensitivity=10;
    public GameObject mainCamerapos;
    public GameObject mainCam;
    public GameObject player;
    public GameObject wallcam;
    public GameObject AimPoint;
   // public GameObject map;

    Vector3 ocpos;
    bool IsAiming=false;
    bool IsGameRunning;

    // Start is called before the first frame update
    void Start()
    {
        print(IsAiming);

        ocpos = mainCamerapos.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        bool IsGameRunning = player.GetComponent<Playercontrol>().IsGameRunning;
        if (IsGameRunning)
        {
            mainCamerapos.transform.LookAt(AimPoint.transform);

            MouseMove();
        AvoidCrossWall();

            x += (float)MoveVector.x * mouseSensitivity *Time.deltaTime;
            y -= (float)MoveVector.y * mouseSensitivity * Time.deltaTime;

            if (x > 360)
                x -= 360;
            else if (x < 0)
                x += 360;

            if (y > 80)
                y = 80;
            else if (y < -80)
                y = -80;

            Rotation = Quaternion.Euler(y, x, 0);
            transform.rotation = Rotation;
           // map.transform.rotation = Quaternion.Euler(90, 0, -transform.rotation.eulerAngles.y);

        
        }
    }
    public void MouseMove()
    {

            MoveVector = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

    }
    private void AvoidCrossWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, mainCamerapos.transform.position- transform.position, out hit, 5))
        {
            if (hit.collider.tag == "Obstacle")
            {
                wallcam.transform.position = hit.point;
                mainCam.transform.position = wallcam.transform.position;
            }
        }
        else
            mainCam.transform.position = mainCamerapos.transform.position;

    }
    public void Aim()
    {
        if (Input.GetMouseButtonDown(1))
        {
            IsAiming = true;
            transform.localPosition =(new Vector3(1.2f,0.3f,1));
        }
        if (Input.GetMouseButtonUp(1))
        {
            IsAiming= false;
            transform.localPosition = (new Vector3(0.5f, 0.3f, 0));

        }

    }
    public void OnSliderValueChanged(float value)
    {
        mouseSensitivity = value;
        print(mouseSensitivity);
    }

}
