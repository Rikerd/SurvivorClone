using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public LevelEnemySpawners enemySpawnerInfo;

    private float currentSpawnTimer = 0f;

    private int currentSpawnPattern = 0;
    private int currentTimeEventSpawn = 0;

    private int currentMiniBossSpawn = 0;

    private enum ScreenEdge { Top, Bottom, Left, Right };

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        for (int spawnPattern = 0; spawnPattern < enemySpawnerInfo.spawnPatterns.Count; spawnPattern++)
        {
            enemySpawnerInfo.spawnPatterns[spawnPattern].enemySpawnRates = enemySpawnerInfo.spawnPatterns[spawnPattern].enemySpawnRates.OrderBy(x => x.rate).ToList();
        }

        currentSpawnTimer = UnityEngine.Random.Range(enemySpawnerInfo.spawnPatterns[currentSpawnPattern].minSpawnTimer, enemySpawnerInfo.spawnPatterns[currentSpawnPattern].maxSpawnTimer);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentSpawnTimer <= 0f)
        {
            int randNumOfEnemies = Random.Range(1, 3);
            for (int numOfEnemy = 0; numOfEnemy < randNumOfEnemies; numOfEnemy++)
            {
                Vector2 positionWorldPoint = FindWorldPositionToSpawn();

                GameObject enemy = ChooseEnemyByRates();
                if (enemy != null)
                {
                    Instantiate(enemy, positionWorldPoint, Quaternion.identity);
                }
            }

            currentSpawnTimer = UnityEngine.Random.Range(enemySpawnerInfo.spawnPatterns[currentSpawnPattern].minSpawnTimer, enemySpawnerInfo.spawnPatterns[currentSpawnPattern].maxSpawnTimer);
        }
        else if (currentSpawnTimer > 0f)
        {
            currentSpawnTimer -= Time.deltaTime;
        }
    }

    public void UpdateSpawnPattern()
    {
        if (currentSpawnPattern + 1 < enemySpawnerInfo.spawnPatterns.Count)
        {
            currentSpawnPattern++;
        }
    }

    public void SpawnMiniBoss()
    {
        MiniBossSpawnData miniBossSpawnData = enemySpawnerInfo.miniBossSpawns[currentMiniBossSpawn];
        for (int numOfMiniBoss = 0; numOfMiniBoss < miniBossSpawnData.numToSpawn; numOfMiniBoss++)
        {
            Vector2 positionWorldPoint = FindWorldPositionToSpawn();
            GameObject enemy = enemySpawnerInfo.miniBossSpawns[currentMiniBossSpawn].miniBoss;
            Instantiate(enemy, positionWorldPoint, Quaternion.identity);
        }
        currentMiniBossSpawn++;
    }

    public void SpawnTimeEventEnemies()
    {
        TimeEventSpawnData currentTimeEventSpawnData = enemySpawnerInfo.timeEventSpawn[currentTimeEventSpawn];

        Vector2 positionWorldPoint = player.transform.position;
        if (currentTimeEventSpawnData.isRandomSpawnLocation)
        {
            positionWorldPoint = FindWorldPositionToSpawn();
        }
        GameObject specialEnemy = currentTimeEventSpawnData.timeEventEnemy;
        Instantiate(specialEnemy, positionWorldPoint, Quaternion.identity);

        currentTimeEventSpawn++;
    }

    private GameObject ChooseEnemyByRates()
    {
        List<EnemySpawnRates> enemySpawnRates = enemySpawnerInfo.spawnPatterns[currentSpawnPattern].enemySpawnRates;
        int rate = Random.Range(0, 100);

        for (int i = 0; i < enemySpawnRates.Count; i++)
        {
            if (rate < enemySpawnRates[i].rate)
            {
                return enemySpawnRates[i].enemy;
            }
        }

        return null;
    }

    private Vector2 FindWorldPositionToSpawn()
    {
        float viewportXCoordinate = 0;
        float viewportYCoordinate = 0;

        ScreenEdge screenEdgeToSpawn = (ScreenEdge)UnityEngine.Random.Range(0, 4);

        if (screenEdgeToSpawn == ScreenEdge.Top)
        {
            viewportXCoordinate = UnityEngine.Random.Range(0f, 1f);
            viewportYCoordinate = 1;
        }
        else if (screenEdgeToSpawn == ScreenEdge.Bottom)
        {
            viewportXCoordinate = UnityEngine.Random.Range(0f, 1f);
            viewportYCoordinate = 0;
        }
        else if (screenEdgeToSpawn == ScreenEdge.Left)
        {
            viewportXCoordinate = 0;
            viewportYCoordinate = UnityEngine.Random.Range(0f, 1f);
        }
        else if (screenEdgeToSpawn == ScreenEdge.Right)
        {
            viewportXCoordinate = 1;
            viewportYCoordinate = UnityEngine.Random.Range(0f, 1f);
        }

        Vector2 positionWorldPoint = Camera.main.ViewportToWorldPoint(new Vector2(viewportXCoordinate, viewportYCoordinate));

        return positionWorldPoint;
    }

    public bool IsLastSpawnPattern()
    {
        return currentSpawnPattern == enemySpawnerInfo.spawnPatterns.Count - 1;
    }

    public float GetCurrentSpawnPatternEndTimer()
    {
        return enemySpawnerInfo.spawnPatterns[currentSpawnPattern].patternEndTimeInSeconds;
    }

    public bool IsLastTimeEventSpawn()
    {
        return currentTimeEventSpawn == enemySpawnerInfo.timeEventSpawn.Count - 1;
    }

    public float GetCurrentTimeEventSpawnTimer()
    {
        return enemySpawnerInfo.timeEventSpawn[currentTimeEventSpawn].timeEventTimeInSeconds;
    }

    public bool IsLastMiniBossSpawn()
    {
        return currentMiniBossSpawn == enemySpawnerInfo.miniBossSpawns.Count - 1;
    }

    public float GetCurrentMiniBossSpawnTimer()
    {
        return enemySpawnerInfo.miniBossSpawns[currentMiniBossSpawn].miniBossTimeInSeconds;
    }
}
