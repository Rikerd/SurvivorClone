using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeEnemyController : EnemyController
{
    private Vector2 direction;
    private Vector3 offset = Vector3.zero;

    protected override void Start()
    {
        base.Start();

        direction = (player.transform.position + offset - transform.position).normalized;
    }

    protected override void FixedUpdate()
    {
        if (isDead)
        {
            DeathAnimation();
            return;
        }

        CheckForLeftOrRightFacing(direction);

        MoveEnemy(rb2d.position + direction * baseGameMoveSpeed * enemyStat.moveSpeedRatio * Time.fixedDeltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetOffsetAmount(Vector3 positionOffset)
    {
        offset = positionOffset;
    }
}
