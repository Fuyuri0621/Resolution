using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewComboConfig",menuName = "ComboSystem/CreateNewComboConfig")]
public class ComboConfig : ScriptableObject
{
    public string animatorStateName;
    public float releaseTime;
    public string effectName;
    public int damagerate;
    public int stunrate;
    public float knockbackrate;
    public float attacklength;
    public float attackwidth;
    public float attackzoffset=0;
    public bool haveHold = false;
}
