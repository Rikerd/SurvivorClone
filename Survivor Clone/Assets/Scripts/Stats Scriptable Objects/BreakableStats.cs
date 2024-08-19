using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Breakable Stat", menuName = "Stats/Breakable")]
public class BreakableStats : ScriptableObject
{
    public int maxHealth;

    public GameObject damageText;

    [SerializeField]
    public List<MultipleDrops> drops;
}

[Serializable]
public class MultipleDrops : Drops
{
    public bool spreadDrop;
    public int minDropAmount;
    public int maxDropAmount;
}

[Serializable]
public class Drops
{
    public GameObject item;

    [Range(0f, 100f)]
    public float rate;
}