using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallSpawner : MonoBehaviour
{
    public enum Orientation { Vertical, Horizontal };

    public Orientation orientation = Orientation.Vertical;
    public int numOfEnemyToSpawn = 20;
    public float length = 3f;
    public float wallSpacing = 5f;
    public GameObject enemy;

    // Start is called before the first frame update
    private void Start()
    {
        Vector3 positiveCenterPoint = Vector3.zero;
        Vector3 negativeCenterPoint = Vector3.zero;
        if (orientation == Orientation.Vertical)
        {
            positiveCenterPoint = transform.position + Vector3.right * wallSpacing;
            negativeCenterPoint = transform.position + Vector3.left * wallSpacing;
        }
        else if (orientation == Orientation.Horizontal)
        {
            positiveCenterPoint = transform.position + Vector3.up * wallSpacing;
            negativeCenterPoint = transform.position + Vector3.down * wallSpacing;
        }


        float nextPostion = length / numOfEnemyToSpawn;
        for (int enemySpawn = 0; enemySpawn < numOfEnemyToSpawn; enemySpawn++)
        {
            if (orientation == Orientation.Vertical)
            {
                Instantiate(enemy, positiveCenterPoint + new Vector3(0, nextPostion * enemySpawn, 0), Quaternion.identity);
                Instantiate(enemy, negativeCenterPoint + new Vector3(0, nextPostion * enemySpawn, 0), Quaternion.identity);
                Instantiate(enemy, positiveCenterPoint - new Vector3(0, nextPostion * enemySpawn, 0), Quaternion.identity);
                Instantiate(enemy, negativeCenterPoint - new Vector3(0, nextPostion * enemySpawn, 0), Quaternion.identity);
            }
            else if (orientation == Orientation.Horizontal)
            {
                Instantiate(enemy, positiveCenterPoint + new Vector3(nextPostion * enemySpawn, 0, 0), Quaternion.identity);
                Instantiate(enemy, negativeCenterPoint + new Vector3(nextPostion * enemySpawn, 0, 0), Quaternion.identity);
                Instantiate(enemy, positiveCenterPoint - new Vector3(nextPostion * enemySpawn, 0, 0), Quaternion.identity);
                Instantiate(enemy, negativeCenterPoint - new Vector3(nextPostion * enemySpawn, 0, 0), Quaternion.identity);
            }

        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (orientation == Orientation.Vertical)
        {
            Vector3 positiveCenterPoint = transform.position + Vector3.right * wallSpacing;
            Vector3 negativeCenterPoint = transform.position + Vector3.left * wallSpacing;
            Gizmos.DrawLine(positiveCenterPoint, positiveCenterPoint + Vector3.up * length);
            Gizmos.DrawLine(negativeCenterPoint, negativeCenterPoint + Vector3.up * length);

            Gizmos.DrawLine(positiveCenterPoint, positiveCenterPoint + Vector3.down * length);
            Gizmos.DrawLine(negativeCenterPoint, negativeCenterPoint + Vector3.down * length);
        }
        else if (orientation == Orientation.Horizontal)
        {
            Vector3 positiveCenterPoint = transform.position + Vector3.up * wallSpacing;
            Vector3 negativeCenterPoint = transform.position + Vector3.down * wallSpacing;
            Gizmos.DrawLine(positiveCenterPoint, positiveCenterPoint + Vector3.right * length);
            Gizmos.DrawLine(negativeCenterPoint, negativeCenterPoint + Vector3.right * length);

            Gizmos.DrawLine(positiveCenterPoint, positiveCenterPoint + Vector3.left * length);
            Gizmos.DrawLine(negativeCenterPoint, negativeCenterPoint + Vector3.left * length);
        }
    }
}
