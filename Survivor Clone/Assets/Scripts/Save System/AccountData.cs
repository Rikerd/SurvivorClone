using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AccountData
{
    public enum UpgradeType
    {
        Damage = 0,
        MaxHealth,
        Armor,
        Projectile,
    }

    public int coins;

    public int[] accountUpgradeTypeLevels = new int[4];

    public AccountData()
    {
        coins = 0;
        accountUpgradeTypeLevels[(int)UpgradeType.Damage] = 0;
        accountUpgradeTypeLevels[(int)UpgradeType.MaxHealth] = 0;
        accountUpgradeTypeLevels[(int)UpgradeType.Armor] = 0;
        accountUpgradeTypeLevels[(int)UpgradeType.Projectile] = 0;
    }
}
