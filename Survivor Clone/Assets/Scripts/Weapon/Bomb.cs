using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Weapon
{
    public AuraStats bombStat;

    private void Start()
    {
        SetMaxCooldown(bombStat.levelStats[currentWeaponLevel].maxCooldown);
    }

    private void Update()
    {
        bool cooldownComplete = base.UpdateCooldownTimer();

        if (cooldownComplete)
        {
            AuraLevelStats currentLevelStats = bombStat.levelStats[currentWeaponLevel];
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, currentLevelStats.radius, LayerMask.GetMask("Enemy"));
            foreach (Collider2D collider in colliders)
            {
                collider.GetComponent<IDamageable>().DamageHealth(currentLevelStats.damage);
            }

            Destroy(gameObject);
        }
    }

    public void SetWeaponLevel(int level)
    {
        currentWeaponLevel = level;

        SetMaxCooldown(bombStat.levelStats[currentWeaponLevel].maxCooldown);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        Gizmos.DrawWireSphere(transform.position, bombStat.levelStats[currentWeaponLevel].radius);
    }
}
