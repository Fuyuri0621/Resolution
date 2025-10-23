using UnityEngine;

public class BattleBarrier : MonoBehaviour
{

    void Start()
    {
        
    }


    void Update()
    {
        if (!GameManager.Instance.isBattling) { Destroy(gameObject); }
    }
}
