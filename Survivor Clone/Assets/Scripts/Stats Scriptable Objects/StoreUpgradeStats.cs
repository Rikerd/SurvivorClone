using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Store Upgrade Stat", menuName = "Stats/Store Upgrade Stat")]
public class StoreUpgradeStats : ScriptableObject
{
    public AccountData.UpgradeType upgradeType;

    public float rateIncrease;
    public List<int> coinCosts;
}
