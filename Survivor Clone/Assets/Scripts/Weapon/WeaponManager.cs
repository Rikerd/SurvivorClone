using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<Weapon> weapons;

    public Weapon GetWeaponToLevel()
    {
        int weaponIndex = Random.Range(0, weapons.Count);
        return weapons[weaponIndex];
    }
}
