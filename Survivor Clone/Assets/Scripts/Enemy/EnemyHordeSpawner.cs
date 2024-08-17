using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHordeSpawner : MonoBehaviour
{
    public int numOfEnemyToSpawn = 20;
    public float radius = 3f;
    public GameObject enemy;

    // Start is called before the first frame update
    private void Start()
    {
        for (int enemySpawn = 0; enemySpawn < numOfEnemyToSpawn; enemySpawn++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * radius;
            GameObject hordeEnemy = Instantiate(enemy, transform.position + randomPosition, Quaternion.identity);
            HordeEnemyController hordeEnemyController = hordeEnemy.GetComponent<HordeEnemyController>();
            hordeEnemyController.SetOffsetAmount(randomPosition);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
