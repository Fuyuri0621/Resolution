using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{

    private static EffectManager _instance;
   public List<GameObject> _effects = new List<GameObject>();

    public static EffectManager Instance
    {
        get { return _instance; }
    }


   public GameObject hitEffect;
    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnHitEffect(Vector3 pos)
    {
        Instantiate(hitEffect,pos,Quaternion.identity);
    }
    public void SpawnEffectByName(string effectname,Transform pos)
    {
        GameObject effect = _effects.Find(x => x.name == effectname);
        GameObject e= Instantiate(effect, pos);
        Destroy(e, 5);
    }
}
