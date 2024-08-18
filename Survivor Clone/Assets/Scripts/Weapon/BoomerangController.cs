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
            List<(Vector3, float)> closestEnemies = FindClosestEnemies(colliders);

            for (int i = 0; i < closestEnemies.Count; i++)
            {
                Quaternion rotation  = Quaternion.LookRotation(Vector3.forward, closestEnemies[i].Item1 - transform.position);
                GameObject projectile = Instantiate(projectileStat.projectile, transform.parent.position, rotation);
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.SetValues(currentLevelStats.damage, projectileStat.moveSpeedRatio, projectileStat.canCrit, currentLevelStats.pierceAmount);
            }
        }
    }

    private List<(Vector3, float)> FindClosestEnemies(Collider2D[] colliders)
    {
        List<(Vector3, float)> enemyDistances = new List<(Vector3, float)>();
        for (int i = 0; i < projectileStat.levelStats[currentWeaponLevel].projectileCount; i++)
        {
            Vector3 randomDirection = Random.insideUnitCircle.normalized;
            enemyDistances.Add((transform.position + randomDirection, Mathf.Infinity));
        }

        foreach (Collider2D collidier in colliders)
        {
            float distance = Vector3.Distance(transform.position, collidier.transform.position);

            foreach ((Vector3, float) enemyDistance in enemyDistances.OrderByDescending(x => x.Item2))
            {
                if (distance < enemyDistance.Item2)
                {
                    enemyDistances.Remove(enemyDistance);
                    enemyDistances.Add((collidier.transform.position, distance));
                    break;
                }
            }
        }

        return enemyDistances;
    }

    public override void LevelUpWeapon()
    {
        base.LevelUpWeapon();
        SetMaxCooldown(projectileStat.levelStats[currentWeaponLevel].maxCooldown);
    }
}
