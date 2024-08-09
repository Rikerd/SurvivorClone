using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public int maxLevel;
    public int maxExp = 10;

    public static ExperienceManager Instance;
    [SerializeField]
    private int currentExp = 0;

    public delegate void ExperienceChangeHandler(int amount);
    public event ExperienceChangeHandler OnExperienceChange;    
    public delegate void LevelChangeHandler(int currentExp, int maxExp);
    public event LevelChangeHandler OnLevelUp;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentExp = 0;
        HUDManager.Instance.InitializeExpBar(maxExp);
    }

    public void AddExperience(int amount)
    {
        if (currentExp + amount >= maxExp)
        {
            currentExp = currentExp + amount - maxExp;
            maxExp *= 2;
            OnLevelUp?.Invoke(currentExp, maxExp);
        }
        else
        {
            currentExp += amount;
            OnExperienceChange?.Invoke(currentExp);
        }
    }
}
