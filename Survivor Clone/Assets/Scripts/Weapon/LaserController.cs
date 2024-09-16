using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : Weapon
{
    public BasicWeaponStats weaponStats;

    public LineRenderer rightLineRender;
    public LineRenderer leftLineRender;

    private float fadeTime = 0.35f;
    private float timer = 0f;

    bool fadeLaser = false;

    private void Start()
    {
        currentWeaponLevel = 0;
        SetMaxCooldown(weaponStats.levelStats[currentWeaponLevel].maxCooldown);

        Color startColor = rightLineRender.startColor;
        Color endColor = rightLineRender.endColor;

        startColor.a = 0;
        endColor.a = 0;

        rightLineRender.startColor = startColor;
        rightLineRender.endColor = endColor;
        leftLineRender.startColor = startColor;
        leftLineRender.endColor = endColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeLaser)
        {
            FadeLaser();
            return;
        }

        bool cooldownComplete = base.UpdateCooldownTimer();

        if (cooldownComplete)
        {
            WeaponLevelStats currentLevelStats = weaponStats.levelStats[currentWeaponLevel];

            RaycastHit2D[] rightEnemies = Physics2D.CircleCastAll(transform.position, 0.9f, Vector2.right, 30f, LayerMask.GetMask("Enemy"));
            foreach (RaycastHit2D enemy in rightEnemies)
            {
                int damage = UnityEngine.Random.Range(currentLevelStats.minDamage, currentLevelStats.maxDamage + 1);
                enemy.collider.gameObject.GetComponent<IDamageable>().DamageHealth(damage);
            }

            RaycastHit2D[] leftEnemies = Physics2D.CircleCastAll(transform.position, 0.9f, Vector2.left, 30f, LayerMask.GetMask("Enemy"));
            foreach (RaycastHit2D enemy in leftEnemies)
            {
                int damage = UnityEngine.Random.Range(currentLevelStats.minDamage, currentLevelStats.maxDamage + 1);
                enemy.collider.gameObject.GetComponent<IDamageable>().DamageHealth(damage);
            }

            Vector3 startRightPosition = transform.position + new Vector3(0.7f, 0, 0);
            Vector3 endRightPosition = transform.position + new Vector3(30f, 0, 0);
            Vector3 startLeftPosition = transform.position + new Vector3(-0.7f, 0, 0);
            Vector3 endLeftPosition = transform.position + new Vector3(-30f, 0, 0);
            rightLineRender.SetPositions(new[] { startRightPosition, endRightPosition });
            leftLineRender.SetPositions(new[] { startLeftPosition, endLeftPosition });
            fadeLaser = true;

            GameManager.Instance.audioSource.PlayOneShot(fireSfx);
        }
    }

    private void FadeLaser()
    {
        timer += Time.deltaTime / fadeTime;
        float alpha = Mathf.Lerp(1f, 0f, timer);

        Color startColor = rightLineRender.startColor;
        Color endColor = rightLineRender.endColor;
        startColor.a = alpha;
        endColor.a = alpha;

        rightLineRender.startColor = startColor;
        rightLineRender.endColor = endColor;
        leftLineRender.startColor = startColor;
        leftLineRender.endColor = endColor;

        if (timer >= 1f)
        {
            timer = 0f;
            fadeLaser = false;
        }
    }

    public override void LevelUpWeapon()
    {
        base.LevelUpWeapon();
        SetMaxCooldown(weaponStats.levelStats[currentWeaponLevel].maxCooldown);
    }
}
