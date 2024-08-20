using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Weapon
{
    public AuraStats bombStat;

    public Transform bombAreaIndicator;

    private void Start()
    {
        SetMaxCooldown(bombStat.levelStats[currentWeaponLevel].maxCooldown);

        bombAreaIndicator.localScale = new Vector2(bombStat.levelStats[currentWeaponLevel].radius, bombStat.levelStats[currentWeaponLevel].radius) * 2;
    }

    private void Update()
    {
        bool cooldownComplete = base.UpdateCooldownTimer();

        if (cooldownComplete)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Explode();
        }
    }

    private void Explode()
    {
        AuraLevelStats currentLevelStats = bombStat.levelStats[currentWeaponLevel];
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, currentLevelStats.radius, LayerMask.GetMask("Enemy"));
        foreach (Collider2D collider in colliders)
        {
            int damage = Random.Range(currentLevelStats.minDamage, currentLevelStats.maxDamage + 1);
            collider.GetComponent<IDamageable>().DamageHealth(damage);
        }

        Destroy(gameObject);
    }

    public void SetWeaponLevel(int level)
    {
        currentWeaponLevel = level;

        bombAreaIndicator.localScale = new Vector2(bombStat.levelStats[currentWeaponLevel].radius, bombStat.levelStats[currentWeaponLevel].radius) * 2;
        SetMaxCooldown(bombStat.levelStats[currentWeaponLevel].maxCooldown);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        Gizmos.DrawWireSphere(transform.position, bombStat.levelStats[currentWeaponLevel].radius);
    }
}
