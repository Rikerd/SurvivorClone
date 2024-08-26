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

        if (numOfWeapons == 0)
        {
            return new List<Weapon>();
        }


        if (activeWeapons.Count >= maxActiveWeapons)
        {
            return CreateActiveWeaponLevelUpList(activeWeapons, numOfWeapons);
        }
        else
        {
            return CreateWeaponLevelUpList(weapons, activeWeapons, numOfWeapons);
        }
    }

    private List<Weapon> CreateActiveWeaponLevelUpList(List<Weapon> weapons, int numOfWeapons)
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

    private List<Weapon> CreateWeaponLevelUpList(List<Weapon> weapons, List<Weapon> activeWeapons, int numOfWeapons)
    {
        List<Weapon> weaponList = new List<Weapon>();
        int weaponsFound = 0;

        HelperFunctions.ShuffleList(ref weapons);
        HelperFunctions.ShuffleList(ref activeWeapons);

        // Favor active weapons first
        foreach (Weapon weapon in activeWeapons)
        {
            if (weapon.GetCurrentWeaponLevel() < 4)
            {
                bool shouldAdd = Random.value > 0.5f;

                if (shouldAdd)
                {
                    weaponList.Add(weapon);
                    weaponsFound++;
                }

                if (weaponsFound == numOfWeapons)
                {
                    return weaponList;
                }
            }
        }

        foreach (Weapon weapon in weapons)
        {
            if (weapon.GetCurrentWeaponLevel() < 4)
            {
                if (!weaponList.Contains(weapon))
                {
                    weaponList.Add(weapon);
                    weaponsFound++;
                }
            }

            if (weaponsFound == numOfWeapons)
            {
                return weaponList;

            }
        }
        return weaponList;
    }

    public void UpdateActiveWeaponList(Weapon weapon)
    {
        activeWeapons.Add(weapon);
    }

    public int GetNumOfActiveWeapons()
    {
        return activeWeapons.Count;
    }
}
