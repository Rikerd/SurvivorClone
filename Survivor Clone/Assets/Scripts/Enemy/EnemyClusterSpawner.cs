using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClusterSpawner : MonoBehaviour
{
    public int numOfEnemyToSpawn = 20;
    public float radius = 3f;
    public GameObject enemy;

    // Start is called before the first frame update
    private void Start()
    {
        for (int enemySpawn = 0; enemySpawn < numOfEnemyToSpawn; enemySpawn++)
        {
            Vector3 randomPosition = Random.insideUnitCircle * radius;
            GameObject clusterEnemy = Instantiate(enemy, transform.position + randomPosition, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
