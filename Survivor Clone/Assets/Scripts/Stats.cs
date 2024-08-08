using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Stat", menuName = "Stats/Characters")]
public class Stats : ScriptableObject
{
    public int maxHealth;
    public float movementSpeed;
    public int damage;
}
