using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(int damage,int stun,float knockback,AllPlayerCharacter attacker);

    public void TakeDamage(int damage, float knockback,Transform attackform);
}


