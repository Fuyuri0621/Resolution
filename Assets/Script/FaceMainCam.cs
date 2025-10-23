using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMainCam : MonoBehaviour
{
    // Start is called before the first frame update

     Camera cam;
    void Start()
    {
        cam = Camera.main;
            }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        }
    }
}
