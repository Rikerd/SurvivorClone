using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aura Spawner Stat", menuName = "Stats/Weapon/Aura Spawner")]
public class AuraSpawnerStats : WeaponStats
{
    public GameObject spawnObject;

    public List<AuraSpawnerLevelStats> levelStats;
}

[Serializable]
public class AuraSpawnerLevelStats : AuraLevelStats
{
    public int spawnAmount;
}
