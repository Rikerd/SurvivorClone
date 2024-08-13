using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private List<Weapon> weapons;

    private void Awake()
    {
        weapons = GetComponentsInChildren<Weapon>().ToList<Weapon>();
    }

    private void Start()
    {
        foreach (Weapon weapon in weapons)
        {
            if (!weapon.isStarterWeapon)
            {
                weapon.DisableWeapon();
            }
            else
            {
                weapon.ActivateWeapon();
            }
        }
    }

    public List<Weapon> GetWeaponsToLevel(int numOfWeapons)
    {
        if (numOfWeapons > weapons.Count)
        {
            numOfWeapons = weapons.Count;
        }

        HelperFunctions.ShuffleList(ref weapons);
        return weapons.GetRange(0, numOfWeapons);
    }
}
