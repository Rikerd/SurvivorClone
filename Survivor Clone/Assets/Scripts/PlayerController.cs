using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour, IDamageable
{
    public PlayerCharacterStats playerStat;
    [field:SerializeField] public int currentHealth { get; set; }
    public int maxHealth { get; set; }

    private Vector2 movement = Vector2.zero;
    private float movementSpeed;

    private Rigidbody2D rb2d;

    private PlayerInput playerInput;
    private InputAction movementAction;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        movementAction = playerInput.actions["Movement"];

        maxHealth = playerStat.maxHealth;
        currentHealth = maxHealth;

        movementSpeed = playerStat.movementSpeed;
    }

    private void Start()
    {
        HUDManager.Instance.UpdateHealthValue(currentHealth, maxHealth);
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

    public void DamageHealth(int damageAmount)
    {
        currentHealth -= damageAmount;

        HUDManager.Instance.UpdateHealthValue(currentHealth, maxHealth);

        if (currentHealth <= 0 )
        {
            currentHealth = 0;
            Death();
        }
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

    // ExperienceManager
    private void HandleLevelUp(int currentExp, int maxExp)
    {
        playerStat.baseAttack += playerStat.attackLevelRate;
    }

    public void LevelUpPlayerHealth()
    {
        maxHealth += playerStat.healthRate;
        currentHealth += playerStat.healthRate;
        HUDManager.Instance.UpdateHealthValue(currentHealth, maxHealth);
    }

    public void LevelUpPlayerMovementSpeed()
    {
        movementSpeed += playerStat.movementSpeedRate;
    }

    public void LevelUpPlayerCritChance()
    {
        playerStat.critChance += playerStat.critChanceRate;
    }
}
