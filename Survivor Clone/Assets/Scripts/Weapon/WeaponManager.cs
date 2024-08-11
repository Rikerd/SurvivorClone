using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<Weapon> weapons;

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
