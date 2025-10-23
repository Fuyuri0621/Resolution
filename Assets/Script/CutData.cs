using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public class CutData : MonoBehaviour
{
    public List<ActorData> actors = new List<ActorData>();
    public List<CamData> cams=new List<CamData>();
    public List<Transform> waypoints = new List<Transform>();
    public UnityEvent onAccomplish;

}


[Serializable]
public class ActorData
{
    public string GameObjectname;
    public Transform transform;
    public Transform startTransform;
}

[Serializable]
public class CamData
{
    public string camName;
    public Transform position;
}


[Serializable]
public class ActorBehaviour
{
    public string GameObjectname;
    public string emoteName;
    public string waypointname;
}
