using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public LevelEnemySpawners enemySpawnerInfo;

    private float currentSpawnTimer = 0f;
    private int currentSpawnPattern = 0;

    private int currentMiniBossPattern = 1;

    private enum ScreenEdge { Top, Bottom, Left, Right };

    // Start is called before the first frame update
    void Start()
    {
        for (int spawnPattern = 0; spawnPattern < enemySpawnerInfo.spawnPatterns.Count; spawnPattern++)
        {
            enemySpawnerInfo.spawnPatterns[spawnPattern].enemySpawnRates = enemySpawnerInfo.spawnPatterns[spawnPattern].enemySpawnRates.OrderBy(x => x.rate).ToList();
        }

        currentSpawnTimer = UnityEngine.Random.Range(enemySpawnerInfo.spawnPatterns[currentSpawnPattern].minSpawnTimer, enemySpawnerInfo.spawnPatterns[currentSpawnPattern].maxSpawnTimer);
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentSpawnTimer <= 0f)
        {
            Vector2 positionWorldPoint = FindWorldPositionToSpawn();

            GameObject enemy = ChooseEnemyByRates();
            if (enemy != null)
            {
                Instantiate(enemy, positionWorldPoint, Quaternion.identity);
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

    public void CheckToSpawnMiniBoss(int miniBossPattern)
    {
        if (currentMiniBossPattern == enemySpawnerInfo.miniBossSpawns.Count())
        {
            currentMiniBossPattern -= enemySpawnerInfo.miniBossSpawns.Count();
        }

        if (miniBossPattern >= enemySpawnerInfo.miniBossSpawns.Count())
        {
            miniBossPattern -= enemySpawnerInfo.miniBossSpawns.Count();
        }

        if (miniBossPattern == currentMiniBossPattern)
        {
            MiniBossSpawnData miniBossSpawnData = enemySpawnerInfo.miniBossSpawns[currentMiniBossPattern - 1];
            for (int numOfMiniBoss = 0; numOfMiniBoss < miniBossSpawnData.numToSpawn; numOfMiniBoss++)
            {
                Vector2 positionWorldPoint = FindWorldPositionToSpawn();
                GameObject enemy = enemySpawnerInfo.miniBossSpawns[currentMiniBossPattern - 1].miniBoss;
                Instantiate(enemy, positionWorldPoint, Quaternion.identity);
                currentMiniBossPattern++;
            }
        }
    }

    public void SpawnEnemyMob()
    {
        Vector2 positionWorldPoint = FindWorldPositionToSpawn();
        int randomEnemyMobIndex = UnityEngine.Random.Range(0, enemySpawnerInfo.specialMobSpawns.Count() - 1);
        GameObject enemyMob = enemySpawnerInfo.specialMobSpawns[randomEnemyMobIndex];
        Instantiate(enemyMob, positionWorldPoint, Quaternion.identity);
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

    public float GetCurrentSpawnPatternEndTimer()
    {
        return enemySpawnerInfo.spawnPatterns[currentSpawnPattern].patternEndTimeInSeconds;
    }
}
