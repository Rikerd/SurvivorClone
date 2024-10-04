using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingEnemyController : EnemyController
{
    private float currentLifeTime = 0f;

    private Vector3 playerOriginalPosition;
    private TemporaryEnemyStats tempEnemyStats;

    protected override void Start()
    {
        base.Start();

        currentLifeTime = 0;

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

    protected override void FixedUpdate()
    {
        if (isDead)
        {
            DeathAnimation();
            return;
        }

        Vector3 direction = (playerOriginalPosition - transform.position).normalized;
        CheckForLeftOrRightFacing(direction);
        MoveEnemy(Vector3.MoveTowards(transform.position, playerOriginalPosition, baseGameMoveSpeed * tempEnemyStats.moveSpeedRatio * Time.fixedDeltaTime));
    }

    public void SetPlayerOriginalPosition(Vector3 position)
    {
        playerOriginalPosition = position;
    }
}
