using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public int maxExp = 10;

    public static ExperienceManager Instance;

    private int currentExp = 0;
    private int currentLevel = 1;

    public delegate void ExperienceChangeHandler(int amount);
    public event ExperienceChangeHandler OnExperienceChange;    
    public delegate void LevelChangeHandler(int currentExp, int maxExp, int currentLevel);
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
            float newMaxExp = maxExp * 1.7f;
            maxExp = (int)(newMaxExp + 0.5f);
            currentLevel++;
            OnLevelUp?.Invoke(currentExp, maxExp, currentLevel);
        }
        else
        {
            currentExp += amount;
            OnExperienceChange?.Invoke(currentExp);
        }
    }
}
