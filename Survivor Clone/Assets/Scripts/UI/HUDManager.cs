using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthText;
    private int maxHealth;

    public Slider expSlider;

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

    public void InitializeHealthBar(int health)
    {
        maxHealth = health;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        healthText.SetText(maxHealth.ToString() + " / " + maxHealth.ToString());
    }

    public void UpdateHealthValue(int health)
    {
        healthSlider.value = health;
        healthText.SetText(health.ToString() + " / " + maxHealth.ToString());
    }

    public void InitializeExpBar(int exp)
    {
        expSlider.maxValue = exp;
        expSlider.value = 0;
    }

    // ExperienceManager
    private void HandleLevelUp(int currentExp, int maxExp)
    {
        expSlider.value = currentExp;
        expSlider.maxValue = maxExp;
    }

    private void HandleExprienceChange(int amount)
    {
        expSlider.value = amount;
    }
}
