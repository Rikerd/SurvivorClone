using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public float maxExp = 10;

    public static ExperienceManager Instance;

    private float currentExp = 0;
    private int currentLevel = 1;

    public delegate void ExperienceChangeHandler(float amount);
    public event ExperienceChangeHandler OnExperienceChange;    
    public delegate void LevelChangeHandler(float currentExp, float maxExp, int currentLevel);
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

    public void AddExperience(float amount)
    {
        amount += amount * GameManager.Instance.GetStoreExperienceMultiplier();

        PassiveItem expPassive = PassiveItemManager.Instance.IsPassiveActiveById(PassiveItemStats.PassiveId.Experience);
        if (expPassive != null)
        {
            BasicPassiveItemStats expPassiveStats = (BasicPassiveItemStats)expPassive.stat;

            amount += amount * expPassiveStats.stats[expPassive.currentLevel].rateIncrease;
        }

        if (currentExp + amount >= maxExp)
        {
            currentExp = currentExp + amount - maxExp;
            maxExp += 10;

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
