using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinalBossController : EnemyController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void BossDeath()
    {
        base.BossDeath();

        GameManager.Instance.TriggerWinSequence();
    }
}
