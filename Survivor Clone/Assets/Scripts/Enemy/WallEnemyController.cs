using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEnemyController : EnemyController
{
    private TemporaryEnemyStats tempEnemyStats;
    private float currentLifeTime = 0f;

    protected override void Start()
    {
        base.Start();

        tempEnemyStats = (enemyStat as TemporaryEnemyStats);
    }

    protected override void Update()
    {
        base.Update();

        currentLifeTime += Time.deltaTime;
        if (currentLifeTime > tempEnemyStats.lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
