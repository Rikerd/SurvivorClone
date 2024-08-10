using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAuraSphere : Weapon
{
    public AuraStats auraStat;

    private int currentLevel = 0;

    // Start is called before the first frame update
    private void Start()
    {
        currentLevel = 0;
        SetMaxCooldown(auraStat.levelStats[currentLevel].maxCooldown);
    }

    // Update is called once per frame
    void Update()
    {
        bool cooldownComplete = base.UpdateCooldownTimer();

        if (cooldownComplete)
        {
            AuraLevelStats currentLevelStats = auraStat.levelStats[currentLevel];
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, currentLevelStats.radius, LayerMask.GetMask("Enemy"));
            foreach (Collider2D collider in colliders)
            {
                collider.GetComponent<IDamageable>().DamageHealth(currentLevelStats.damage);
            }
        }
    }
}
