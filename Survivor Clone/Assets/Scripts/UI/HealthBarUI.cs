using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Slider healthSlider;
    private TMP_Text healthText;
    private int maxHealth;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeHealthBar(int health)
    {
        healthSlider = GetComponent<Slider>();

        maxHealth = health;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        healthText = GetComponentInChildren<TMP_Text>();
        healthText.SetText(maxHealth.ToString() + " / " + maxHealth.ToString());
    }

    public void UpdateHealthValue(int health)
    {
        healthSlider.value = health;
        healthText.SetText(health.ToString() + " / " + maxHealth.ToString());
    }
}
