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
        MovementSpeed,
        Experience,
        CoinDrop,
        PickUpRadius,
    }

    public int coins;

    public int[] accountUpgradeTypeLevels = new int[8];

    public AccountData()
    {
        coins = 0;
        accountUpgradeTypeLevels[(int)UpgradeType.Damage] = 0;
        accountUpgradeTypeLevels[(int)UpgradeType.MaxHealth] = 0;
        accountUpgradeTypeLevels[(int)UpgradeType.Armor] = 0;
        accountUpgradeTypeLevels[(int)UpgradeType.Projectile] = 0;
        accountUpgradeTypeLevels[(int)UpgradeType.MovementSpeed] = 0;
        accountUpgradeTypeLevels[(int)UpgradeType.Experience] = 0;
        accountUpgradeTypeLevels[(int)UpgradeType.CoinDrop] = 0;
        accountUpgradeTypeLevels[(int)UpgradeType.PickUpRadius] = 0;
    }
}
