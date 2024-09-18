using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthText;

    public TMP_Text killCountText;

    public TMP_Text coinText;

    public TMP_Text timerText;

    public Slider expSlider;
    public TMP_Text levelText;

    public TMP_Text gameOverKillsText;
    public TMP_Text gameOverCoinsText;

    public TMP_Text victoryKillsText;
    public TMP_Text victoryCoinsText;

    public static HUDManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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

    private void OnEnable()
    {
        // ExperienceManager callbacks
        ExperienceManager.Instance.OnLevelUp += HandleLevelUp;
        ExperienceManager.Instance.OnExperienceChange += HandleExprienceChange;
    }

    private void OnDisable()
    {
        // ExperienceManager callbacks
        ExperienceManager.Instance.OnLevelUp -= HandleLevelUp;
        ExperienceManager.Instance.OnExperienceChange -= HandleExprienceChange;
    }

    public void UpdateHealthValue(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthText.SetText(currentHealth.ToString() + " / " + maxHealth.ToString());
    }

    public void InitializeExpBar(float exp)
    {
        expSlider.maxValue = exp;
        expSlider.value = 0;

        levelText.SetText("Lv. 1");
    }

    public void UpdateKillCountValue(int count)
    {
        killCountText.SetText("Kills: " + count.ToString());
    }

    public void UpdateCoinValue(int count)
    {
        coinText.SetText("Coins: " + count.ToString());
    }

    public void UpdateTimeValue(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        string timeInMinSecs = string.Format("{0:0}:{1:00}", minutes, seconds);
        timerText.SetText(timeInMinSecs);
    }

    public void SetGameOverPanelValues(int playerKillCount, int coinsEarned)
    {
        gameOverKillsText.SetText("Kills: " + playerKillCount.ToString());
        gameOverCoinsText.SetText("Coins Gained: " + coinsEarned.ToString());
    }

    public void SetVictoryPanelValues(int playerKillCount, int coinsEarned)
    {
        victoryKillsText.SetText("Kills: " + playerKillCount.ToString());
        victoryCoinsText.SetText("Coins Gained: " + coinsEarned.ToString());
    }

    // ExperienceManager
    private void HandleLevelUp(float currentExp, float maxExp, int currentLevel)
    {
        expSlider.value = currentExp;
        expSlider.maxValue = maxExp;

        levelText.SetText("Lv. " + currentLevel.ToString());
    }

    private void HandleExprienceChange(float amount)
    {
        expSlider.value = amount;
    }
}
