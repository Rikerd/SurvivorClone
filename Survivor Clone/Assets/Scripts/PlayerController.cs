using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour, IDamageable
{
    public Stats playerStat;
    [field:SerializeField] public int currentHealth { get; set; }

    private Vector2 movement = Vector2.zero;

    private Rigidbody2D rb2d;

    private PlayerInput playerInput;
    private InputAction movementAction;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        movementAction = playerInput.actions["Movement"];

        currentHealth = playerStat.maxHealth;
    }

    private void Start()
    {
        HUDController.hud.InitializeHealthBar(playerStat.maxHealth);
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
    }

    private void OnDisable()
    {
        // Disable movement callbacks
        movementAction.Disable();
        movementAction.started -= OnMovement;
        movementAction.performed -= OnMovement;
        movementAction.canceled -= OnMovement;
    }

    public void DamageHealth(int damageAmount)
    {
        currentHealth -= damageAmount;

        HUDController.hud.UpdateHealthValue(currentHealth);

        if (currentHealth <= 0 )
        {
            currentHealth = 0;
            Death();
        }
    }

    public void Death()
    {
        GameManager.gm.TriggerDeathSequence();
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        input.Normalize();
        movement = input * playerStat.movementSpeed;
    }
}
