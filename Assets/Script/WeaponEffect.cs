using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffect : MonoBehaviour
{
    Collider weaponcol;
    public GameObject effectprefab;


    private void Awake()
    {
        weaponcol = GetComponent<Collider>();
    }
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       Vector3 pos = weaponcol.ClosestPointOnBounds(other.transform.position);

     //   EffectManager.Instance.SpawnHitEffect(pos);
    }
}
