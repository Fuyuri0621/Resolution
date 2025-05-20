using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackPoint : MonoBehaviour
{
    // Start is called before the first frame update
    StageEventManager stageManager;
    [SerializeField] Transform[] tp;
    Transform player;
    void Start()
    {
        stageManager =gameObject.GetComponentInParent<StageEventManager>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HackPlayer")
        {
            stageManager.AddPoint();
            GoToNextPoint();
        }
    }

    private void GoToNextPoint()
    {
        Transform traget = tp[UnityEngine.Random.Range(0, tp.Length)];
        while (Vector3.Distance(player.position, traget.position) < 5) { traget = tp[UnityEngine.Random.Range(0, tp.Length)]; }
        
        transform.position=traget.position;
    }
}
