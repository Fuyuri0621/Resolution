using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Mission/MissionTable", fileName = "MissionTable")]
public class MissionTable : ScriptableObject
{
    public List<MissionTableItem> MissionList = new List<MissionTableItem>();

}


public enum MissionType
{
    Interact,
    GetItem,
    Killememy,
    ToPosition


}



