using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEventManager : MonoBehaviour
{
    [SerializeField] public StageData stageData;
    [SerializeField] BarrierManger enemiesManger;
    [SerializeField] GameObject player;
    [SerializeField] Transform startpos;
    [SerializeField] int point;
    [SerializeField] int endPoint;
   public HackPC PC;
    StageTime stageTime;
    int evemtIndexer;



    private void Awake()
    {
        stageTime = GetComponent<StageTime>();
    }

    private void Start()
    {
        player.transform.position = startpos.position;
    }
    private void Update()
    {
        if (evemtIndexer >= stageData.stageEvents.Count) { return; }

        if (stageTime.time > stageData.stageEvents[evemtIndexer].time)
        {
            switch (stageData.stageEvents[evemtIndexer].eventType)
            {
                case StageEventType.SpawnEnemy:
                        SpawnEnemy();
                    break;

                case StageEventType.SpawnObject:
                    SpawnObject();
                    break;

                case StageEventType.WinStage:
                    Winstage();
                    break;

                case StageEventType.loseStage:
                    Losestage();
                    break;
            }
            Debug.Log(stageData.stageEvents[evemtIndexer].message);

            
            
            evemtIndexer += 1;
        }
    }

    public void Losestage()
    {
        evemtIndexer = 0;
        stageTime.time = 0;
        player.transform.position = startpos.position;
        
    }

    private void Winstage()
    {
       
        Debug.Log("Unlock");
        GameManager.Instance.SetIsControlling(true);
        PC.HackComplete();
       // Destroy(gameObject);
    }

    private void SpawnEnemy()
    {
        StageEvent currentEvent = stageData.stageEvents[evemtIndexer];
        enemiesManger.AddGroupToSpawn(currentEvent.barrierToSpawn, currentEvent.count);

        if (currentEvent.isRepeatedEvent == true)
        {
            enemiesManger.AddRepeatedSpawn(currentEvent);
        }
        
    }
    public void AddPoint()
    {
        point += 1;
        if(point >= endPoint)
        {
            Winstage();
        }
    }
    private void SpawnObject()
    {
      
    }
}
