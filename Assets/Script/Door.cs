using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    Animator animator;
    Playercontrol1 playercontrol1;
    [SerializeField] GameObject ring;
    [SerializeField] GameObject unlockAera;
    [SerializeField] GameObject point;

    [SerializeField] GameObject Fcanva;

    [SerializeField] bool islock;
    [SerializeField] bool isunlocking=false;
    [SerializeField] int unlockingspeed=50;
    int unlockedAngle;
    float unlockingAngle;

    [SerializeField] float releseTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isunlocking&&releseTime==0)
        {

            unlockingAngle += 100 * unlockingspeed * Time.deltaTime;
            if (unlockingAngle >= 360) {unlockingAngle = 0;}
            point.transform.rotation =Quaternion.Euler(0,0,unlockingAngle);

        }
        if (releseTime != 0) { releseTime -= Time.deltaTime; }
        if(releseTime < 0) { releseTime = 0;}

    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.IsControlling)
        { playercontrol1 = other.gameObject.GetComponent<Playercontrol1>(); if (islock) { Fcanva.SetActive(true); } }
       

        if (other.gameObject.tag == "Player"&&!islock) { animator.SetBool("isopen", true); }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && !islock) { animator.SetBool("isopen", false); }

      //  if (GameManager.Instance.IsControlling) Fcanva.SetActive(false);
    }

    public void Unlockedd()
    {

        if (Fcanva.activeInHierarchy)
        {
            GameManager.Instance.IsControlling = false;
            ring.SetActive(true);
            unlockedAngle = Random.Range(25, 330);
            unlockAera.transform.rotation = Quaternion.Euler(0, 0, unlockedAngle);
            isunlocking = true;
            Fcanva.SetActive(false);

        }
    }
    public void Check()
    {

        if (isunlocking)
        {
            isunlocking = false;
            if (unlockingAngle < unlockedAngle + 20 && unlockingAngle > unlockedAngle - 20) { islock = false; ring.SetActive(false); GameManager.Instance.IsControlling = true; animator.SetBool("isopen", true); }
            else
            { releseTime = 2; isunlocking = true; }
        }   
    }


}
