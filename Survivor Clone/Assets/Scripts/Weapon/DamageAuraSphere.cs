using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAuraSphere : Weapon
{
    public AuraStats auraStat;

    // Start is called before the first frame update
    private void Start()
    {
        SetMaxCooldown(auraStat.levelStats[currentWeaponLevel].maxCooldown);

        transform.localScale = new Vector2(auraStat.levelStats[currentWeaponLevel].radius, auraStat.levelStats[currentWeaponLevel].radius) * 2;
    }

    // Update is called once per frame
    private void Update()
    {
        bool cooldownComplete = base.UpdateCooldownTimer();

        if (cooldownComplete)
        {
            AuraLevelStats currentLevelStats = auraStat.levelStats[currentWeaponLevel];
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, currentLevelStats.radius, LayerMask.GetMask("Enemy"));
            foreach (Collider2D collider in colliders)
            {
                int damage = Random.Range(currentLevelStats.minDamage, currentLevelStats.maxDamage + 1);
                collider.GetComponent<IDamageable>().DamageHealth(damage);
            }
        }
    }

    public override void LevelUpWeapon()
    {
        base.LevelUpWeapon();

        transform.localScale = new Vector2(auraStat.levelStats[currentWeaponLevel].radius, auraStat.levelStats[currentWeaponLevel].radius) * 2;
        SetMaxCooldown(auraStat.levelStats[currentWeaponLevel].maxCooldown);
    }

    public void SetWeaponLevel(int level)
    {
        currentWeaponLevel = level;

        transform.localScale = new Vector2(auraStat.levelStats[currentWeaponLevel].radius, auraStat.levelStats[currentWeaponLevel].radius) * 2;
        SetMaxCooldown(auraStat.levelStats[currentWeaponLevel].maxCooldown);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, auraStat.levelStats[currentWeaponLevel].radius);
    }
}
