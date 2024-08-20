using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Enemy Spawner", menuName = "Enemy Spawner Rates")]
public class LevelEnemySpawners : ScriptableObject
{
    public List<SpawnPattern> spawnPatterns;

    public List<MiniBossSpawnData> miniBossSpawns;

    public List<TimeEventSpawnData> timeEventSpawn;
}

[Serializable]
public class SpawnPattern
{
    public float patternEndTimeInSeconds;
    public float minSpawnTimer;
    public float maxSpawnTimer;
    public List<EnemySpawnRates> enemySpawnRates;
}

[Serializable]
public class EnemySpawnRates
{
    public GameObject enemy;
    public float rate;
}

[Serializable]
public class MiniBossSpawnData
{
    public GameObject miniBoss;
    public int numToSpawn;
    public float miniBossTimeInSeconds;
}


[Serializable]
public class TimeEventSpawnData
{
    public GameObject timeEventEnemy;
    public float timeEventTimeInSeconds;
    public bool isRandomSpawnLocation = true;
}