using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobEnemyController : EnemyController
{
    private Vector2 direction;

    protected override void Start()
    {
        base.Start();

        direction = (player.transform.position - transform.position).normalized;
    }

    protected override void FixedUpdate()
    {
        MoveEnemy(rb2d.position + direction * enemyStat.movementSpeed * Time.fixedDeltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
