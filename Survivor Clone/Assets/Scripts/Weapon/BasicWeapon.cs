using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicWeapon : Weapon
{
    public ProjectileStats projectileStat;

    public Transform aimController;
    public float tripleProjectileWaitTime = 0.2f;

    // Start is called before the first frame update
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
            StartCoroutine(SpawnTripleProjectile());
        }
    }

    private IEnumerator SpawnTripleProjectile()
    {
        SetPauseCooldown(true);

        Quaternion lastAimRotation = aimController.rotation;
        ProjectileLevelStats currentLevelStats = projectileStat.levelStats[currentWeaponLevel];

        for (int i = 0; i < currentLevelStats.projectileCount; i++)
        {
            GameObject projectile = Instantiate(projectileStat.projectile, aimController.parent.position, lastAimRotation);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            GameManager.Instance.audioSource.PlayOneShot(fireSfx);
            projectileScript.SetValues(currentLevelStats.minDamage, currentLevelStats.maxDamage, projectileStat.moveSpeedRatio, projectileStat.canCrit, currentLevelStats.pierceAmount);
            yield return new WaitForSeconds(tripleProjectileWaitTime);
        }

        SetPauseCooldown(false);
    }

    public override void LevelUpWeapon()
    {
        base.LevelUpWeapon();
        SetMaxCooldown(projectileStat.levelStats[currentWeaponLevel].maxCooldown);
    }
}
