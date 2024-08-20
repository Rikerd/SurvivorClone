using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Passive Stat", menuName = "Stats/Passive/Basic Passive")]
public class BasicPassiveItemStats : PassiveItemStats
{
    public List<PassiveLevelStats> stats;
}
