using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;


[CreateAssetMenu(fileName = "AttatkData", menuName = "Scriptable Objects/AttatkData")]
public class AttatkData : ScriptableObject
{
    public List<Attackinfo> info = new List<Attackinfo>();
}
[Serializable]
public class Attackinfo 
{
    public AttackID attackID;
    public int damageRate;
    public int stunRate;
    public float knockbackRate;
    public Vector3 size;
    public float zoffset;
}

public enum AttackID
{
    BossAttackA1,
    BossAttackA2,
    BossAttackA3,
    BossAttackB1,
    BossAttackB2,
    BossAttackB3,
    BossAttackC1,
    BossAttackC2,
    BossAttackC3,
    BossShoot
}
