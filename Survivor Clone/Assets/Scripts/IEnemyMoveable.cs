using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveable
{
    Rigidbody2D rb2d { get; set; }
    Transform playerTransform { get; set; }

    bool isFacingRight { get; set; }

    void MoveEnemy(Vector2 movement);
    void CheckForLeftOrRightFacing(Vector2 movement);
}
