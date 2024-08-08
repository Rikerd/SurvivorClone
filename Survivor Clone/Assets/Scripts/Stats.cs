using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "Stats")]
public class Stats : ScriptableObject
{
    public int maxHealth;
    public float movementSpeed;
    public int damage;
}
