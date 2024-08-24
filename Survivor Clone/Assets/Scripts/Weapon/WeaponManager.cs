using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int maxActiveWeapons = 5;

    private List<Weapon> weapons;
    private List<Weapon> activeWeapons = new List<Weapon>();

    public static WeaponManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

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


        if (activeWeapons.Count >= maxActiveWeapons)
        {
            return CreateWeaponLevelUpList(activeWeapons, numOfWeapons);
        }
        else
        {
            return CreateWeaponLevelUpList(weapons, numOfWeapons);
        }
    }

    private List<Weapon> CreateWeaponLevelUpList(List<Weapon> weapons, int numOfWeapons)
    {
        List<Weapon> weaponList = new List<Weapon>();
        int weaponsFound = 0;

        HelperFunctions.ShuffleList(ref weapons);

        foreach (Weapon weapon in weapons)
        {
            if (weapon.GetCurrentWeaponLevel() < 4)
            {
                weaponList.Add(weapon);
                weaponsFound++;
            }

            if (weaponsFound == numOfWeapons)
            {
                break;
            }
        }

        return weaponList;
    }

    public void UpdateActiveWeaponList(Weapon weapon)
    {
        activeWeapons.Add(weapon);
    }
}
