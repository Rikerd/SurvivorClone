using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Level Up Info", menuName = "Level Up Info")]
public class LevelUpInfo : ScriptableObject
{
    public string upgradeName;
    public string description;
}
