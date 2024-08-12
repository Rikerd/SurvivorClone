using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DaggerThrower : Weapon
{
    public ProjectileStats projectileStat;

    public int currentAngle = 360;

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

            for (int i = 0; i < currentLevelStats.projectileCount; i++)
            {
                Vector3 eulerRotation = new Vector3(0, 0, currentAngle - (45 * i));
                GameObject projectile = Instantiate(projectileStat.projectile, transform.parent.position, Quaternion.Euler(eulerRotation));
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.SetValues(currentLevelStats.damage, projectileStat.movementSpeed, projectileStat.canCrit);
            }
            currentAngle -= 45;
            if (currentAngle == 0)
            {
                currentAngle = 360;
            }
        }
    }

    public override void LevelUpWeapon()
    {
        base.LevelUpWeapon();
        SetMaxCooldown(projectileStat.levelStats[currentWeaponLevel].maxCooldown);
    }
}
