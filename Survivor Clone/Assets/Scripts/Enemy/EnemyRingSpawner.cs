using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRingSpawner : MonoBehaviour
{
    public int numOfEnemyToSpawn = 20;
    public float radius = 3f;
    public GameObject enemy;

    // Start is called before the first frame update
    private void Start()
    {
        float angle = 0;
        float nextAngle = 2 * Mathf.PI / numOfEnemyToSpawn;
        for (int enemySpawn = 0; enemySpawn < numOfEnemyToSpawn; enemySpawn++)
        {
            Vector3 ringPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            GameObject ringEnemy = Instantiate(enemy, transform.position + ringPosition, Quaternion.identity);

            angle += nextAngle;
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
