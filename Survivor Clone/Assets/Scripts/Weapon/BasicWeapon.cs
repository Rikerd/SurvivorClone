using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : Weapon
{
    public Transform aimController;
    public float tripleProjectileWaitTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
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
        for (int i = 0; i < 3; i++)
        {
            GameObject projectile = Instantiate(weaponStat.projectile, aimController.position, aimController.rotation);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.SetWeaponStats(weaponStat);
            yield return new WaitForSeconds(tripleProjectileWaitTime);
        }
    }
}
