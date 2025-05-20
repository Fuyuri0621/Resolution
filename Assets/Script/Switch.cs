using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Switch : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject characterA;
    [SerializeField] GameObject characterB;
    [SerializeField] GameObject characterACam;
    [SerializeField] GameObject characterBCam;

    Playercontrol1 playerControlA;
    Playercontrol1 playerControlB;


    void Start()
    {
        playerControlA = characterA.GetComponent<Playercontrol1>();
        playerControlB = characterB.GetComponent<Playercontrol1>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
