using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CinemachineShakeManager;

public interface IDamageable
{
    public void TakeDamage(int damage,int stun,float knockback,AllPlayerCharacter attacker, ShakeStrength shakeStrength= ShakeStrength.LIGHT);

    public void TakeDamage(int damage, float knockback,Transform attackform,ShakeStrength shakeStrength);
}


