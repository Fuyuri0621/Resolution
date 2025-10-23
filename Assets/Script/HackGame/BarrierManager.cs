using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;




public class BarrierManger : MonoBehaviour
{
    [SerializeField] GameObject barrier;
    [SerializeField] Transform[] spawnPointUP;
    [SerializeField] Transform[] spawnPointRight;
    [SerializeField] float spawntime;

    Vector2 spawnarea;



    List<BarrierSpawnGroup> enemiesSpawnGroupsList;
    List<BarrierSpawnGroup> repeatedSpawnGroupList;


    private void Start()
    {



    }

    private void Update()
    {
        ProcessSpawn();
        ProcessRepeatedSpawnGroups();
    }

    private void ProcessRepeatedSpawnGroups()
    {
        if (repeatedSpawnGroupList == null) { return; }
        for (int i = repeatedSpawnGroupList.Count - 1; i >= 0; i--)
        {
            repeatedSpawnGroupList[i].repeatTimer -= Time.deltaTime;
            if (repeatedSpawnGroupList[i].repeatTimer < 0)
            {
                repeatedSpawnGroupList[i].repeatTimer = repeatedSpawnGroupList[i].timeBetweenSpawn;
                AddGroupToSpawn(repeatedSpawnGroupList[i].barrierType, repeatedSpawnGroupList[i].count);
                repeatedSpawnGroupList[i].repeatCount -= 1;
                if (repeatedSpawnGroupList[i].repeatCount <= 0)
                {
                    repeatedSpawnGroupList.RemoveAt(i);
                }
            }
        }
    }

    private void ProcessSpawn()
    {
        if (enemiesSpawnGroupsList == null) { return; }

        if (enemiesSpawnGroupsList.Count > 0)
        {
            SpawnEnemy(enemiesSpawnGroupsList[0].barrierType);
            enemiesSpawnGroupsList[0].count -= 1;
            if (enemiesSpawnGroupsList[0].count <= 0)
            {
                enemiesSpawnGroupsList.RemoveAt(0);
            }
        }
    }



    public void AddGroupToSpawn(BarrierType barrierTospawn, int count)
    {
        BarrierSpawnGroup newGroupToSpawn = new BarrierSpawnGroup(barrierTospawn, count);

        if (enemiesSpawnGroupsList == null) { enemiesSpawnGroupsList = new List<BarrierSpawnGroup>(); }

        enemiesSpawnGroupsList.Add(newGroupToSpawn);
    }

    public void SpawnEnemy(BarrierType barrierToSpawn)
    {
        Vector3 position;
        float f = UnityEngine.Random.value > 0.5f ? -1f : 1f;
        if (f == 1)
        {
             position = spawnPointRight[UnityEngine.Random.Range(0,spawnPointRight.Length)].position;
            float a = UnityEngine.Random.value > 0.5f ? position.x*= -1 : 1f;
        }
        else
        {
            position = spawnPointUP[UnityEngine.Random.Range(0, spawnPointRight.Length)].position;
            float a = UnityEngine.Random.value > 0.5f ? position.z *= -1 : 1f;
        }
        




        GameObject newBarrier = Instantiate(barrier);
        newBarrier.transform.position = position;
        BarrierControl newBarrierCompoent = newBarrier.GetComponent<BarrierControl>();
        newBarrierCompoent.SetStats(barrierToSpawn,transform.position);



        newBarrier.transform.parent = transform;


    }


    public void AddRepeatedSpawn(StageEvent stageEvent)
    {
        BarrierSpawnGroup repeatSpawnGroup = new BarrierSpawnGroup(stageEvent.barrierToSpawn, stageEvent.count);
        repeatSpawnGroup.SetRepeatSpawn(stageEvent.repeatEverySeconds, stageEvent.repeatCount);

        if (repeatedSpawnGroupList == null)
        {
            repeatedSpawnGroupList = new List<BarrierSpawnGroup>();
        }
        repeatedSpawnGroupList.Add(repeatSpawnGroup);
    }


}

public class BarrierSpawnGroup
{
    public BarrierType barrierType;
    public int count;
    public float repeatTimer;
    public float timeBetweenSpawn;
    public int repeatCount;

    public BarrierSpawnGroup(BarrierType barrierType, int count)
    {
        this.barrierType = barrierType;
        this.count = count;

    }
    public void SetRepeatSpawn(float timeBetweenSpawns, int repeatCount)
    {
        this.timeBetweenSpawn = timeBetweenSpawns;
        this.repeatCount = repeatCount;
        repeatTimer = timeBetweenSpawn;
    }
}
