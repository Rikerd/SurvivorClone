using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    public PlayerCharacterStats playerStat;
    [field:SerializeField] public int currentHealth { get; set; }
    public int maxHealth { get; set; }

    private Vector2 movement = Vector2.zero;
    private float movementSpeed;

    private float critChance;

    private Rigidbody2D rb2d;

    private PlayerInput playerInput;
    private InputAction movementAction;

    private Slider onPlayerHealthBar;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        movementAction = playerInput.actions["Movement"];

        maxHealth = playerStat.maxHealth;
        currentHealth = maxHealth;

        movementSpeed = playerStat.movementSpeed;

        critChance = playerStat.critChance;

        onPlayerHealthBar = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        HUDManager.Instance.UpdateHealthValue(currentHealth, maxHealth);

        onPlayerHealthBar.maxValue = maxHealth;
        onPlayerHealthBar.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + movement * Time.fixedDeltaTime);
    }

    private void OnEnable()
    {
        // Enable movement callbacks
        movementAction.Enable();
        movementAction.started += OnMovement;
        movementAction.performed += OnMovement;
        movementAction.canceled += OnMovement;

        // ExperienceManager callbacks
        ExperienceManager.Instance.OnLevelUp += HandleLevelUp;
    }

    private void OnDisable()
    {
        // Disable movement callbacks
        movementAction.Disable();
        movementAction.started -= OnMovement;
        movementAction.performed -= OnMovement;
        movementAction.canceled -= OnMovement;

        // ExperienceManager callbacks
        ExperienceManager.Instance.OnLevelUp -= HandleLevelUp;
    }

    public void DamageHealth(int damageAmount, bool isCrit = false)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0 )
        {
            currentHealth = 0;
            Death();
        }
        HUDManager.Instance.UpdateHealthValue(currentHealth, maxHealth);
        onPlayerHealthBar.maxValue = maxHealth;
        onPlayerHealthBar.value = currentHealth;
    }

    public void HealHealth(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        HUDManager.Instance.UpdateHealthValue(currentHealth, maxHealth);
        onPlayerHealthBar.maxValue = maxHealth;
        onPlayerHealthBar.value = currentHealth;
    }

    public void Death()
    {
        GameManager.Instance.TriggerDeathSequence();
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        input.Normalize();
        movement = input * movementSpeed;
    }

    public float GetCritChance()
    {
        return critChance;
    }

    // ExperienceManager
    private void HandleLevelUp(int currentExp, int maxExp, int currentLevel)
    {
        playerStat.baseAttack += playerStat.attackLevelRate;
    }

    public void LevelUpPlayerHealth()
    {
        maxHealth += playerStat.healthRate;
        currentHealth += playerStat.healthRate;
        HUDManager.Instance.UpdateHealthValue(currentHealth, maxHealth);
        onPlayerHealthBar.maxValue = maxHealth;
        onPlayerHealthBar.value = currentHealth;
    }

    public void LevelUpPlayerMovementSpeed()
    {
        movementSpeed += playerStat.movementSpeedRate;
    }

    public void LevelUpPlayerCritChance()
    {
        critChance += playerStat.critChanceRate;
    }
}
