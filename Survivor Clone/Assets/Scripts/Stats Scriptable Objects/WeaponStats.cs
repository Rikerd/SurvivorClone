using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Stat", menuName = "Stats/Weapon")]
public class WeaponStats : ScriptableObject
{
    public float movementSpeed;
    public int damage;
    public float maxCooldown;
    public float sizeScale;
    public GameObject projectile;

    public bool canCrit;
}
