using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stat", menuName = "Stats/Enemy")]
public class EnemyStats : Stats
{
    public int damage;

    public float exp;
    public GameObject expOrb;
}
