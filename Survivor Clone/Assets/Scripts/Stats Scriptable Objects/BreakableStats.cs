using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Breakable Stat", menuName = "Stats/Breakable")]
public class BreakableStats : ScriptableObject
{
    public int maxHealth;

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