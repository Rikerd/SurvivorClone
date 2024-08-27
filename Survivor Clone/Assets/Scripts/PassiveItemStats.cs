using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Passive Stat", menuName = "Stats/Passive/Passive")]
public class PassiveItemStats : ScriptableObject
{    public enum PassiveId
    {
        Experience,
        MovementSpeed,
        CriticalChance,
        Damage,
        Armor,
        CoinMultiplier,
    }

    public string passiveName;
    public string description;
    public Sprite uiSprite;

    public PassiveId id;
}


[Serializable]
public class PassiveLevelStats
{
    public float rateIncrease;
}
