using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stat", menuName = "Stats/Enemy")]
public class EnemyStats : Stats
{
    public int damage;

    public float exp;
    public int numOfCoinSpawnAmount;
    public GameObject expOrb;
    public GameObject coin;

    public bool isMiniBoss = false;
}
