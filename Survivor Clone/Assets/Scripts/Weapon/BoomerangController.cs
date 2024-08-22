using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoomerangController : Weapon
{
    public ProjectileStats projectileStat;

    public float radius = 5f;    

    private void Start()
    {
        currentWeaponLevel = 0;
        SetMaxCooldown(projectileStat.levelStats[currentWeaponLevel].maxCooldown);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        bool cooldownComplete = base.UpdateCooldownTimer();

        if (cooldownComplete)
        {
            ProjectileLevelStats currentLevelStats = projectileStat.levelStats[currentWeaponLevel];

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemy"));
            List<(Vector3, float)> closestEnemies = HelperFunctions.FindClosestEnemies(colliders, currentLevelStats.projectileCount + GameManager.Instance.GetStoreProjectileAmount(), transform.position, radius);

            foreach ((Vector3, float) enemy in closestEnemies)
            {
                Quaternion rotation  = Quaternion.LookRotation(Vector3.forward, enemy.Item1 - transform.position);
                GameObject projectile = Instantiate(projectileStat.projectile, transform.parent.position, rotation);
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.SetValues(currentLevelStats.minDamage, currentLevelStats.maxDamage, projectileStat.moveSpeedRatio, projectileStat.canCrit, currentLevelStats.pierceAmount);
            }
            GameManager.Instance.audioSource.PlayOneShot(fireSfx);
        }
    }

    public override void LevelUpWeapon()
    {
        base.LevelUpWeapon();
        SetMaxCooldown(projectileStat.levelStats[currentWeaponLevel].maxCooldown);
    }
}
