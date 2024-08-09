using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public int currentHealth { get; set; }
    public int maxHealth { get; set; }

    public void DamageHealth(int damageAmount, bool isCrit = false);

    public void Death();
}
