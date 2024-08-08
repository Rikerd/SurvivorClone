using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderData;

public class Weapon : MonoBehaviour
{
    public WeaponStats weaponStat;

    private float currentCooldown;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        currentCooldown = weaponStat.maxCooldown;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool UpdateCooldownTimer()
    {
        if (currentCooldown <= 0f)
        {
            currentCooldown = weaponStat.maxCooldown;
            return true;
        }
        else if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
        }

        return false;
    }
}
