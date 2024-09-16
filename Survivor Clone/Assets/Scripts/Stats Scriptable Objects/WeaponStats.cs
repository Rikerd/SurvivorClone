using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : ScriptableObject
{
    public bool canCrit;
}

[Serializable]
public class WeaponLevelStats
{
    public int minDamage;
    public int maxDamage;
    public float maxCooldown;
}