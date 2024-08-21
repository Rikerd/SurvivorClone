using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPotionController : Weapon
{
    public AuraSpawnerStats poisonStat;

    // Start is called before the first frame update
    private void Start()
    {
        SetMaxCooldown(poisonStat.levelStats[currentWeaponLevel].maxCooldown);
    }

    // Update is called once per frame
    private void Update()
    {
        bool cooldownComplete = base.UpdateCooldownTimer();

        if (cooldownComplete)
        {
            AuraSpawnerLevelStats currentWeaponLevelStat = poisonStat.levelStats[currentWeaponLevel];
            for (int numToSpawn = 0; numToSpawn < currentWeaponLevelStat.spawnAmount; numToSpawn++)
            {
                Vector3 spread = Random.insideUnitCircle * currentWeaponLevelStat.radius;

                GameObject poison = Instantiate(poisonStat.spawnObject, transform.position + spread, Quaternion.identity);
                poison.GetComponent<PoisonPotion>().SetWeaponLevel(currentWeaponLevel);
            }
        }
    }

    public override void LevelUpWeapon()
    {
        base.LevelUpWeapon();

        SetMaxCooldown(poisonStat.levelStats[currentWeaponLevel].maxCooldown);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        Gizmos.DrawWireSphere(transform.position, poisonStat.levelStats[currentWeaponLevel].radius);
    }
}
