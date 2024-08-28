using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class HelperFunctions
{
    public static void ShuffleList<T>(ref List<T> list)
    {
        int count = list.Count;
        int lastIndex = count - 1;
        for (int i = 0; i < lastIndex; i++)
        {
            int randomIndex = Random.Range(i, count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public static Vector2 RandomUnitVector()
    {
        float random = Random.Range(0f, 260f);
        return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    }

    public static List<(Vector3, float)> FindClosestEnemies(Collider2D[] colliders, int numOfEnemies, Vector3 position, float spawnRadius)
    {
        List<(Vector3, float)> enemyDistances = new List<(Vector3, float)>();
        for (int i = 0; i < numOfEnemies; i++)
        {
            Vector3 randomDirection = Random.insideUnitCircle.normalized;
            float randomRadius = Random.Range(1f, spawnRadius);
            enemyDistances.Add((position + randomDirection * randomRadius, Mathf.Infinity));
        }

        foreach (Collider2D collidier in colliders)
        {
            float distance = Vector3.Distance(position, collidier.transform.position);

            foreach ((Vector3, float) enemyDistance in enemyDistances.OrderByDescending(x => x.Item2))
            {
                if (distance < enemyDistance.Item2)
                {
                    enemyDistances.Remove(enemyDistance);
                    enemyDistances.Add((collidier.transform.position, distance));
                    break;
                }
            }
        }

        return enemyDistances;
    }

    public static int UpdateProjectileCount(int initialSpawnCount)
    {
        int projectileCount = initialSpawnCount + GameManager.Instance.GetStoreProjectileAmount();

        PassiveItem projectilePassive = PassiveItemManager.Instance.IsPassiveActiveById(PassiveItemStats.PassiveId.Projectile);
        if (projectilePassive != null)
        {
            BasicPassiveItemStats projectilePassiveStats = (BasicPassiveItemStats)projectilePassive.stat;

            projectileCount += (int)projectilePassiveStats.stats[projectilePassive.currentLevel].rateIncrease;
        }

        return projectileCount;
    }
}
