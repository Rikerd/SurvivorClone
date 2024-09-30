using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : Weapon
{
    public ShieldStats weaponStats;

    private SpriteRenderer spriteRenderer;

    private int currentCharges;
    private bool isInvincible;
    private float invincibiltyTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        currentWeaponLevel = 0;
        currentCharges = 1; // Spawn with first charge
        isInvincible = true;
        SetMaxCooldown(weaponStats.levelStats[currentWeaponLevel].maxCooldown);

        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSpriteColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvincible)
        {
            invincibiltyTimer -= Time.deltaTime;

            if (invincibiltyTimer <= 0f)
            {
                isInvincible = false;
            }
        }

        if (currentCharges == weaponStats.levelStats[currentWeaponLevel].maxCharges)
        {
            return;
        }

        bool cooldownComplete = base.UpdateCooldownTimer();

        if (cooldownComplete)
        {
            currentCharges++;
            UpdateSpriteColor();
        }
    }

    public override void LevelUpWeapon()
    {
        base.LevelUpWeapon();
        SetMaxCooldown(weaponStats.levelStats[currentWeaponLevel].maxCooldown);
    }

    public void UseShieldCharge()
    {
        if (currentCharges > 0 && !isInvincible)
        {
            currentCharges--;
            invincibiltyTimer = weaponStats.levelStats[currentWeaponLevel].invincibilityTime;
            isInvincible = true;

            UpdateSpriteColor();

            GameManager.Instance.audioSource.PlayOneShot(fireSfx);
        }
    }

    public bool ShieldActive()
    {
        return isInvincible;
    }

    private void UpdateSpriteColor()
    {
        spriteRenderer.enabled = true;
        switch (currentCharges)
        {
            case 0:
                spriteRenderer.enabled = false;
                break;
            case 1:
                spriteRenderer.color = new Color(0, 0.95f, 1);
                break;
            case 2:
                spriteRenderer.color = Color.green;
                break;
            case 3:
                spriteRenderer.color = Color.yellow;
                break;
        }
    }
}
