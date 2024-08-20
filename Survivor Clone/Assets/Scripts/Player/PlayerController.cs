using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private float moveSpeedRatio;
    private float baseGameMoveSpeed;

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

        moveSpeedRatio = playerStat.moveSpeedRatio;

        critChance = playerStat.critChance;

        onPlayerHealthBar = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        HUDManager.Instance.UpdateHealthValue(currentHealth, maxHealth);

        baseGameMoveSpeed = GameManager.Instance.baseGameMoveSpeed;

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
    }

    private void OnDisable()
    {
        // Disable movement callbacks
        movementAction.Disable();
        movementAction.started -= OnMovement;
        movementAction.performed -= OnMovement;
        movementAction.canceled -= OnMovement;
    }

    public void DamageHealth(int damageAmount, bool isCrit = false)
    {
        int passiveArmor = 0;
        PassiveItem armorPassive = PassiveItemManager.Instance.IsPassiveActiveById(PassiveItemStats.PassiveId.Armor);
        if (armorPassive != null)
        {
            BasicPassiveItemStats armorPassiveStats = (BasicPassiveItemStats)armorPassive.stat;

            passiveArmor = (int)armorPassiveStats.stats[armorPassive.currentLevel].rateIncrease;
        }

        damageAmount -= passiveArmor;
        if (damageAmount <= 0)
        {
            damageAmount = 1;
        }

        TMP_Text damageText = Instantiate(playerStat.damageText, transform.position, Quaternion.identity).GetComponent<TMP_Text>();
        damageText.SetText(damageAmount.ToString());
        damageText.color = Color.red;

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

        float passiveMoveSpeedRatio = 0f;
        PassiveItem moveSpeedPassive = PassiveItemManager.Instance.IsPassiveActiveById(PassiveItemStats.PassiveId.MovementSpeed);
        if (moveSpeedPassive != null)
        {
            BasicPassiveItemStats moveSpeedPassiveStats = (BasicPassiveItemStats)moveSpeedPassive.stat;

            passiveMoveSpeedRatio = moveSpeedPassiveStats.stats[moveSpeedPassive.currentLevel].rateIncrease;
        }

        movement = input * baseGameMoveSpeed * (moveSpeedRatio + passiveMoveSpeedRatio);
    }

    public float GetCritChance()
    {
        float passiveCritChance = 0f;
        PassiveItem critSpeedPassive = PassiveItemManager.Instance.IsPassiveActiveById(PassiveItemStats.PassiveId.CriticalChance);
        if (critSpeedPassive != null)
        {
            BasicPassiveItemStats critPassiveStats = (BasicPassiveItemStats)critSpeedPassive.stat;

            passiveCritChance = critPassiveStats.stats[critSpeedPassive.currentLevel].rateIncrease;
        }

        return critChance + passiveCritChance;
    }
}
