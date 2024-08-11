using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public float minSpawnTimer = 1f;
    public float maxSpawnTimer = 2f;

    public LevelEnemySpawners enemySpawnerInfo;

    private float currentSpawnTimer = 0f;
    private int spawnPattern = 0;

    private enum ScreenEdge { Top, Bottom, Left, Right };

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentSpawnTimer <= 0f)
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

            Vector2 posWS = Camera.main.ViewportToWorldPoint(new Vector2(viewportXCoordinate, viewportYCoordinate));

            Instantiate(enemySpawnerInfo.spawnPatterns[spawnPattern].enemySpawnRates[0].enemy, posWS, Quaternion.identity);

            currentSpawnTimer = UnityEngine.Random.Range(minSpawnTimer, maxSpawnTimer);
        }
        else if (currentSpawnTimer > 0f)
        {
            currentSpawnTimer -= Time.deltaTime;
        }
    }

    public void UpdateDifficulty(int difficulty)
    {
        if (difficulty >= enemySpawnerInfo.spawnPatterns.Count)
        {
            return;
        }
        spawnPattern = difficulty;
    }
}
