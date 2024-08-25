using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Store Upgrade Stat Cost", menuName = "Stats/Store Upgrade Stat Cost")]
public class StoreUpgradeStatCosts : ScriptableObject
{
    public AccountData.UpgradeType upgradeType;
    public string upgradeName;
    public string upgradeDescription;

    public List<int> coinCosts;
}
