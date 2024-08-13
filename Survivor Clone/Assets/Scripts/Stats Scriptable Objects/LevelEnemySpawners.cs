using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Enemy Spawner", menuName = "Enemy Spawner Rates")]
public class LevelEnemySpawners : ScriptableObject
{
    public List<SpawnPattern> spawnPatterns;

    public List<MiniBossSpawnData> miniBossSpawns;
}

[Serializable]
public class SpawnPattern
{
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
}
