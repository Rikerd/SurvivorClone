using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPotion : Weapon
{
    public SpawnedAuraStats auraStat;

    private float currentLifeTime = 0f;
    private bool spawning = true;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (spawning)
        {
            return;
        }

        bool cooldownComplete = base.UpdateCooldownTimer();

        currentLifeTime -= Time.deltaTime;

        if (cooldownComplete)
        {
            AuraLevelStats currentLevelStats = auraStat.levelStats[currentWeaponLevel];
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, currentLevelStats.radius, LayerMask.GetMask("Enemy"));
            foreach (Collider2D collider in colliders)
            {
                int damage = Random.Range(currentLevelStats.minDamage, currentLevelStats.maxDamage + 1);
                collider.GetComponent<IDamageable>().DamageHealth(damage);
            }
        }

        if (currentLifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public override void LevelUpWeapon()
    {
        base.LevelUpWeapon();

        transform.localScale = new Vector2(auraStat.levelStats[currentWeaponLevel].radius, auraStat.levelStats[currentWeaponLevel].radius) * 2;
        SetMaxCooldown(auraStat.levelStats[currentWeaponLevel].maxCooldown);
    }

    public void SetWeaponLevel(int level)
    {
        currentWeaponLevel = level;

        spawning = true;
    }

    public void FinishSpawning()
    {
        SetMaxCooldown(0);
        transform.localScale = new Vector2(auraStat.levelStats[currentWeaponLevel].radius, auraStat.levelStats[currentWeaponLevel].radius) * 2;
        currentLifeTime = auraStat.levelStats[currentWeaponLevel].lifeTime;

        GameManager.Instance.audioSource.PlayOneShot(fireSfx);

        spawning = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, auraStat.levelStats[currentWeaponLevel].radius);
    }
}
