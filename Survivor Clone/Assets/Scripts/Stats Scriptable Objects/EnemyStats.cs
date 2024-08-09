using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stat", menuName = "Stats/Enemy")]
public class EnemyStats : Stats
{
    public int damage;

    [SerializeField]
    public List<Drops> drops;
}

[Serializable]
public class Drops
{
    public GameObject item;

    [Range(0f, 100f)]
    public float rate;
}
