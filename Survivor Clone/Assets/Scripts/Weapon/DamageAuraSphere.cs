using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAuraSphere : Weapon
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        bool cooldownComplete = base.UpdateCooldownTimer();

        if (cooldownComplete)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f, LayerMask.GetMask("Enemy"));
            foreach (Collider2D collider in colliders)
            {
                collider.GetComponent<IDamageable>().DamageHealth(weaponStat.damage);
            }
        }
    }
}
