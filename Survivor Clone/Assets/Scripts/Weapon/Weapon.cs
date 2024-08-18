using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.ShaderData;

public class Weapon : MonoBehaviour
{
    public bool isStarterWeapon = false;
    public LevelUpInfo levelUpInfo;

    private float currentCooldown;
    private float maxCooldown;

    protected bool pauseCooldown;

    protected int currentWeaponLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        pauseCooldown = false;
    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool UpdateCooldownTimer()
    {
        if (pauseCooldown)
        {
            return false;
        }

        if (currentCooldown <= 0f)
        {
            currentCooldown = maxCooldown;
            return true;
        }
        else if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
        }

        return false;
    }

    protected void SetMaxCooldown(float max)
    {
        maxCooldown = max;
        currentCooldown = max;
    }

    public virtual void LevelUpWeapon()
    {
        currentWeaponLevel++;
    }

    public void ActivateWeapon()
    {
        gameObject.SetActive(true);
        WeaponManager.Instance.UpdateActiveWeaponList(this);
        GameManager.Instance.UpdateWeaponHUDUI(levelUpInfo.uiSprite);
    }

    public void DisableWeapon()
    {
        gameObject.SetActive(false);
    }

    public int GetCurrentWeaponLevel()
    {
        return currentWeaponLevel;
    }

    public void SetPauseCooldown(bool isPaused)
    {
        pauseCooldown = isPaused;
    }
}
