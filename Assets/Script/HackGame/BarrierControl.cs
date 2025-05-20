using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SocialPlatforms;

enum BarrierSpawnDirection
{
    Up,
    Down,
    Left,
    Right
}
public enum BarrierType
{
    Block,
    Damage,


}

public class BarrierControl : MonoBehaviour
{
    Vector3 origin;
    public float speed=5;
    public BarrierType barrierType;
    Vector3 movevector;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.x < origin.x)
        {
            movevector = Vector3.down;
        }
        else
        {
            movevector = Vector3.up;
        }
        if(transform.position.z >= origin.z+18)
        {

            transform.Rotate(new Vector3(0, 0, 90));
        }else if(transform.position.z <= -18)
        {

            transform.Rotate(new Vector3(0, 0, -90));
        }
        Destroy(gameObject,15);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(movevector*speed*Time.deltaTime);
    }

    public void SetStats(BarrierType type,Vector3 o)
    {
        barrierType = type;
        origin = o;

    }

    
}
