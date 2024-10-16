using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DaggerThrower : Weapon
{
    public ProjectileStats projectileStat;

    private int currentAngle = 360;

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

            int projectileCount = HelperFunctions.UpdateProjectileCount(currentLevelStats.projectileCount);

            for (int projectileNum = 0; projectileNum < projectileCount; projectileNum++)
            {
                Vector3 eulerRotation = new Vector3(0, 0, currentAngle - (18 * projectileNum));
                GameObject projectileObject = Instantiate(projectileStat.projectile, transform.parent.position, Quaternion.Euler(eulerRotation));
                Projectile projectileScript = projectileObject.GetComponent<Projectile>();
                projectileScript.SetValues(currentLevelStats.minDamage, currentLevelStats.maxDamage, projectileStat.moveSpeedRatio, projectileStat.canCrit, currentLevelStats.pierceAmount);
            }
            GameManager.Instance.audioSource.PlayOneShot(fireSfx);

            currentAngle -= 18;
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
