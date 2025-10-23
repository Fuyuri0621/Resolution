using UnityEngine;

public class EnemySpawnner : MonoBehaviour
{
    [SerializeField] bool bytime;
    [SerializeField] float spawnTime;
    float timer;
    [SerializeField] GameObject[] enemytype;
   public int spawnwave;
    public float spawntime=20;
    GameObject traget;
    bool canspawn;


    public void SpawnEnemy()
    {
       int rnd = Random.Range(0, enemytype.Length);
        
        traget = Instantiate(enemytype[rnd],transform.position ,transform.rotation);
        traget.GetComponent<EnemyControl>().SetTarget(GameManager.Instance.GetCtrolingCharacter());
        traget.GetComponent<NavToTarget>().targetCharacter = GameManager.Instance.controlingCharacter;
        GameManager.Instance.AddtBattleEmemies(traget);
        spawnwave--;
        if (spawnwave == 0) { GameManager.Instance.RemovetBattleEmemies(gameObject); Destroy(gameObject); }

    }

    private void Start()
    {
        GameManager.Instance.AddtBattleEmemies(gameObject);
    }
    private void Update()
    {
        
        if (bytime) { if (Time.time > timer) { SpawnEnemy(); timer = Time.time + spawntime; } }
        if (canspawn) { SpawnEnemy(); }


       

        canspawn = false; // 預設不允許

        foreach (GameObject spawnnsr in GameManager.Instance.battlingEmemies)
        {
            EnemySpawnner enemySpawnner = spawnnsr.GetComponent<EnemySpawnner>();
            if (enemySpawnner == null) continue;

            if (enemySpawnner.traget != null)
            {
                canspawn = false;
                break;
            }
            else
            {
                canspawn = true;
            }
        }
    }
}
