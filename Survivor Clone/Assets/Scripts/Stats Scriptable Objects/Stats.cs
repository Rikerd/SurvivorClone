using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Stat", menuName = "Stats/Basic Stat")]
public class Stats : ScriptableObject
{
    public int maxHealth;
    public float moveSpeedRatio;

    public GameObject damageText;
}
