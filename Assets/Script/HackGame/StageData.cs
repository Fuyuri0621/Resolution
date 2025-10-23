using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StageEventType
{
    SpawnEnemy,
    loseStage,
    SpawnObject,
    WinStage
}


[Serializable]
public class StageEvent
{
    public StageEventType eventType;

    public float time;
    public string message;

    public BarrierType barrierToSpawn;
    public GameObject objectToSpawn;

    public int count;

    public bool isRepeatedEvent;
    public float repeatEverySeconds;
    public int repeatCount;
}

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    public int stageNum;
    public List<StageEvent> stageEvents;
}
