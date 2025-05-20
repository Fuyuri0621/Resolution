using UnityEngine;

public class EnemySpawnner : MonoBehaviour
{
    [SerializeField] bool bytime;
    [SerializeField] float spawnTime;
    float timer;
    [SerializeField] GameObject[] enemytype;
   public int spawntime;
    GameObject traget;

    
    public void SpawnEnemy()
    {
       int rnd = Random.Range(0, enemytype.Length);
        
        traget = Instantiate(enemytype[rnd],transform.position ,transform.rotation);
        traget.GetComponent<EnemyControl>().SetTarget(GameManager.Instance.GetCtrolingCharacter());
        traget.GetComponent<NavToTarget>().targetCharacter = GameManager.Instance.controlingCharacter;
        GameManager.Instance.AddtBattleEmemies(traget);
        spawntime--;
        if (spawntime == 0) { GameManager.Instance.RemovetBattleEmemies(gameObject); Destroy(gameObject); }

    }

    private void Start()
    {
        GameManager.Instance.AddtBattleEmemies(gameObject);
    }
    private void Update()
    {
        if (bytime) { if (Time.time > timer) { SpawnEnemy();timer = Time.time + spawnTime; } }
        if (traget == null) { SpawnEnemy(); }
    }
}
