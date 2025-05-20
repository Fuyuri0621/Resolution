using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHeavyComboConfig", menuName = "ComboSystem/CreateNewHeavyComboConfig")]
public class HeavyComboConfig : ScriptableObject
{
    public string comboName;
    public List<ComboConfig> heavyComboConfig = new List<ComboConfig>();
}