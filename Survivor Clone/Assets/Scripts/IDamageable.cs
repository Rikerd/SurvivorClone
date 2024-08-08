using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public int currentHealth { get; set; }

    public void DamageHealth(int damageAmount);

    public void Death();
}
