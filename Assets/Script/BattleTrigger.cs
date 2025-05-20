using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class BattleTrigger : MonoBehaviour
{
    [SerializeField] GameObject barrier;
    [SerializeField] bool rotate =false;
    [SerializeField] Vector3[] barriersLocate;
    [SerializeField] GameObject spawnner;
    [SerializeField] Vector3[] spawnnerLocate;
    [SerializeField] int spawnWave = 2;

 //   List<GameObject> spawnedobj = new List<GameObject>();
    private void Awake()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Spawnbarrier();
            Spawnspawnner();

            gameObject.SetActive(false);
            
        }
    }

    private void Spawnbarrier()
    {
        for (int i = 0; i < barriersLocate.Length; i++)
        {
            GameObject o = Instantiate(barrier, transform.position + barriersLocate[i], Quaternion.identity);
            if (rotate) { o.transform.Rotate(Vector3.up * 90); }
        }
    }
    private void Spawnspawnner()
    {
        for (int i = 0; i < spawnnerLocate.Length; i++)
        {
            GameObject o = Instantiate(spawnner, transform.position + spawnnerLocate[i], Quaternion.identity);
           o.GetComponent<EnemySpawnner>().spawntime = spawnWave;

        }
    }


    private void OnDrawGizmosSelected()
    {
        if (barriersLocate == null || barriersLocate.Length == 0) return;

        Gizmos.color = Color.red;
        for (int i = 0; i < barriersLocate.Length; i++)
        {
            Gizmos.DrawSphere(transform.position + barriersLocate[i], 0.2f); 
        }

        if (spawnnerLocate == null || spawnnerLocate.Length == 0) return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < spawnnerLocate.Length; i++)
        {
            Gizmos.DrawSphere(transform.position + spawnnerLocate[i], 0.2f);
        }
    }

}
