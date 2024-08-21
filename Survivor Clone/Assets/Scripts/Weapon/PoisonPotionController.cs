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
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, currentWeaponLevelStat.radius, LayerMask.GetMask("Enemy"));
            List<(Vector3, float)> closestEnemies = HelperFunctions.FindClosestEnemies(colliders, currentWeaponLevelStat.spawnAmount, transform.position, currentWeaponLevelStat.radius);

            foreach ((Vector3, float) enemy in closestEnemies)
            {
                GameObject poison = Instantiate(poisonStat.spawnObject, enemy.Item1, Quaternion.identity);
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
