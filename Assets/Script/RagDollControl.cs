using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RagDollControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (var temp in this.transform.GetComponentsInChildren<BoxCollider>())
        {



            temp.isTrigger = true;



        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RagDollON()
    {
        if (GetComponentInParent<Animator>().enabled == false) return;

        GetComponentInParent<Animator>().enabled = false;

        foreach (var temp in this.transform.GetComponentsInChildren<BoxCollider>())
        {



            temp.isTrigger = false;



        }

        foreach (var temp in this.transform.GetComponentsInChildren<Rigidbody>())
        {


            if (temp.name == "head")
                temp.isKinematic = true;
            else
                temp.isKinematic = false;


        }
        

    }

}
