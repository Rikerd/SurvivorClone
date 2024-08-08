using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Stat", menuName = "Stats/Characters")]
public class Stats : ScriptableObject
{
    public int maxHealth;
    public float movementSpeed;
    public int damage;

    [SerializeField]
    public List<Drops> drops;
}

[Serializable]
public class Drops
{
    public GameObject item;

    [Range (0f, 100f)]
    public float rate;
}
