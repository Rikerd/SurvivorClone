using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Store Upgrade Stat Rates", menuName = "Stats/Store Upgrade Stat Rate")]
public class StoreUpgradeStatRates : ScriptableObject
{
    public float damageMultiplierRate;
    public float healthMultiplierRate;
    public int armorRate;
    public int projectileRate;
    public float movementSpeedRate;
    public float experienceMultiplierRate;
    public int coinMultiplierRate;
    public float pickUpRadiusRate;
}
