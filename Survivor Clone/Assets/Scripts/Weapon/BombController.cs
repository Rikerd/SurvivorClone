using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : Weapon
{
    public AuraSpawnerStats bombStat;

    // Start is called before the first frame update
    private void Start()
    {
        SetMaxCooldown(bombStat.levelStats[currentWeaponLevel].maxCooldown);
    }

    // Update is called once per frame
    private void Update()
    {
        bool cooldownComplete = base.UpdateCooldownTimer();

        if (cooldownComplete)
        {
            AuraSpawnerLevelStats currentWeaponLevelStat = bombStat.levelStats[currentWeaponLevel];
            for (int numToSpawn = 0; numToSpawn < currentWeaponLevelStat.spawnAmount; numToSpawn++)
            {
                Vector3 spread = Random.insideUnitCircle.normalized * currentWeaponLevelStat.radius;

                GameObject bomb = Instantiate(bombStat.spawnObject, transform.position, Quaternion.identity);
                bomb.GetComponent<Bomb>().SetWeaponLevel(currentWeaponLevel);
                bomb.GetComponent<Bomb>().StartSpawnAnimation(transform.position + spread);
            }
        }
    }

    public override void LevelUpWeapon()
    {
        base.LevelUpWeapon();

        SetMaxCooldown(bombStat.levelStats[currentWeaponLevel].maxCooldown);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, bombStat.levelStats[currentWeaponLevel].radius);
    }
}
