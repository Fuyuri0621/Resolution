using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSelectCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Checkinteract();
    }

    private void Checkinteract()
    {

        Debug.DrawRay(transform.position, Vector3.forward,Color.green, 300);
        RaycastHit hit;
        InteractableObject @object = null;
        if (Physics.Raycast(transform.position, transform.position - transform.position, out hit, 300))
        {
            if (hit.collider.GetComponent<InteractableObject>() != null)
            {

                @object = hit.collider.GetComponent<InteractableObject>();
                if (!@object.isSelect)
                { @object.isSelect = true; }
            }
            else if (@object != null)
            {
                @object.isSelect = false;
                @object = null;
            }
        }
    }
}
