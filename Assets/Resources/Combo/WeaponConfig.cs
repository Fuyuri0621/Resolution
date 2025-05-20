using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewWeaponConfig",menuName = "ComboSystem/CreateNewWeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    public List<ComboConfig> lightComboConfig = new List<ComboConfig>();
    public List<ComboConfig> heavyComboConfig = new List<ComboConfig>();
}
