using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance;
    public static EventManager Instance
    {
        get { return _instance; }
    }
    List<EnemySpawnner> enemySpawnner;
    private void Awake()
    {
        _instance =this;
    }
    public void StartEnemySwan(EnemySpawnner spawnner)
    {
        enemySpawnner.Add(spawnner);
    }
}
