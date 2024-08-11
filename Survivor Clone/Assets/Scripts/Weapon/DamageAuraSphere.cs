using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAuraSphere : Weapon
{
    public AuraStats auraStat;

    // Start is called before the first frame update
    private void Start()
    {
        currentWeaponLevel = 0;
        SetMaxCooldown(auraStat.levelStats[currentWeaponLevel].maxCooldown);
    }

    // Update is called once per frame
    void Update()
    {
        bool cooldownComplete = base.UpdateCooldownTimer();

        if (cooldownComplete)
        {
            AuraLevelStats currentLevelStats = auraStat.levelStats[currentWeaponLevel];
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, currentLevelStats.radius, LayerMask.GetMask("Enemy"));
            foreach (Collider2D collider in colliders)
            {
                collider.GetComponent<IDamageable>().DamageHealth(currentLevelStats.damage);
            }
        }
    }
}
