using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable, IEnemyMoveable
{
    public EnemyStats enemyStat;

    public float collisionDamageDelayTimer = 0.5f;

    public int currentHealth { get; set; }
    public int maxHealth { get; set; }

    public Rigidbody2D rb2d { get; set; }
    public bool isFacingRight { get; set; }
    public GameObject player { get; set; }

    private bool isCollidingWithPlayer = false;
    private float currentCollisionDamageDelayTimer;
    private SpriteRenderer spriteRenderer;

    protected float baseGameMoveSpeed;

    private float deathAnimationDuration = 0.5f;
    private float timer = 0.0f;
    protected bool isDead = false;
    private Vector3 deathPosition = Vector3.zero;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        maxHealth = enemyStat.maxHealth;
        currentHealth = enemyStat.maxHealth;
        rb2d = GetComponent<Rigidbody2D>();
        isCollidingWithPlayer = false;
        spriteRenderer = GetComponent<SpriteRenderer>();

        baseGameMoveSpeed = GameManager.Instance.baseGameMoveSpeed;
    }

    protected virtual void Update()
    {
        if (isDead)
        {
            return;
        }

        if (isCollidingWithPlayer)
        {
            if (currentCollisionDamageDelayTimer <= 0)
            {
                player.GetComponent<IDamageable>().DamageHealth(enemyStat.damage);
                currentCollisionDamageDelayTimer = collisionDamageDelayTimer;
            }
            else
            {
                currentCollisionDamageDelayTimer -= Time.deltaTime;
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (isDead)
        {
            DeathAnimation();

            if (timer >= 1f)
            {
                Destroy(gameObject);
            }
            return;
        }

        Vector3 direction = (player.transform.position - transform.position).normalized;
        CheckForLeftOrRightFacing(direction);
        MoveEnemy(Vector3.MoveTowards(transform.position, player.transform.position, baseGameMoveSpeed * enemyStat.moveSpeedRatio * Time.fixedDeltaTime));
    }

    public void DeathAnimation()
    {
        timer += Time.deltaTime / deathAnimationDuration;
        float alphaLerp = Mathf.Lerp(1f, 0f, timer);

        float fadeDirection = isFacingRight ? -1f : 1f;
        float movementLerp = fadeDirection * Mathf.Lerp(0f, 1f, timer);

        Color spriteColor = spriteRenderer.color;
        spriteColor.a = alphaLerp;
        spriteRenderer.color = spriteColor;

        MoveEnemy(deathPosition + new Vector3(movementLerp, 0, 0));
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            isCollidingWithPlayer = true;
            currentCollisionDamageDelayTimer = 0f;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            isCollidingWithPlayer = false;
        }
    }

    #region Health Functions
    public void DamageHealth(int damageAmount, bool isCrit = false)
    {
        if (isDead)
        {
            return;
        }

        float passiveDamageMultiplier = 0;
        PassiveItem damagePassive = PassiveItemManager.Instance.IsPassiveActiveById(PassiveItemStats.PassiveId.Damage);
        if (damagePassive != null)
        {
            BasicPassiveItemStats damagePassiveStats = (BasicPassiveItemStats)damagePassive.stat;

            passiveDamageMultiplier = damagePassiveStats.stats[damagePassive.currentLevel].rateIncrease;
        }

        int additionaDamageAmount = 0;
        float storeDamageUpgradeMultiplier = GameManager.Instance.GetStoreDamageMultiplier();
        if (isCrit)
        {
            int damageAmountBeforeCrit = damageAmount / 2;
            if (storeDamageUpgradeMultiplier > 0)
            {
                damageAmountBeforeCrit += Mathf.RoundToInt(damageAmountBeforeCrit * storeDamageUpgradeMultiplier);
            }

            if (passiveDamageMultiplier > 0)
            {
                additionaDamageAmount = Mathf.RoundToInt(damageAmountBeforeCrit * passiveDamageMultiplier);
            }

            damageAmount = (damageAmountBeforeCrit + additionaDamageAmount) * 2;
        }
        else
        {
            if (storeDamageUpgradeMultiplier > 0)
            {
                damageAmount += Mathf.RoundToInt(damageAmount * storeDamageUpgradeMultiplier);
            }

            if (passiveDamageMultiplier > 0)
            {
                additionaDamageAmount = Mathf.RoundToInt(damageAmount * passiveDamageMultiplier);
            }

            damageAmount += additionaDamageAmount;
        }

        currentHealth -= damageAmount;
        TMP_Text damageText = Instantiate(enemyStat.damageText, transform.position, Quaternion.identity).GetComponent<TMP_Text>();
        damageText.SetText(damageAmount.ToString());
        if (isCrit)
        {
            damageText.color = Color.yellow;
            damageText.fontSize *= 1.2f;
        }

        damageText.GetComponent<DamageTextFader>().SetFadeDirection(isFacingRight);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }
    }

    public void Death()
    {
        isDead = true;
        deathPosition = transform.position;
        GetComponent<Collider2D>().enabled = false;

        GameManager.Instance.IncrementKillCount();

        if (enemyStat.isBossType)
        {
            BossDeath();
        }
        else
        {
            int rand = Random.Range(0, 100);

            if (rand <= 95)
            {
                GameObject expOrb = Instantiate(enemyStat.expOrb, transform.position, Quaternion.identity);
                expOrb.GetComponent<ExperiencePickUp>().SetExperienceAmount(enemyStat.exp);
            }
        }
    }

    protected virtual void BossDeath()
    {
        GameObject expOrb = Instantiate(enemyStat.expOrb, transform.position, Quaternion.identity);
        expOrb.GetComponent<ExperiencePickUp>().SetExperienceAmount(enemyStat.exp);

        for (int coinSpawnAmount = 0; coinSpawnAmount < enemyStat.numOfCoinSpawnAmount; coinSpawnAmount++)
        {
            Vector3 spread = Vector3.zero;

            float radius = Random.Range(0.8f, 3.8f);
            spread = Random.insideUnitCircle * radius;

            Instantiate(enemyStat.coin, transform.position + spread, Quaternion.identity);
        }
    }
    #endregion Health Functions

    public void MoveEnemy(Vector2 movement)
    {
        rb2d.MovePosition(movement);
    }

    public void CheckForLeftOrRightFacing(Vector2 movement)
    {
        if (movement.x > 0)
        {
            isFacingRight = true;
            spriteRenderer.flipX = false;
        }
        else
        {
            isFacingRight = false;
            spriteRenderer.flipX = true;
        }
    }
}
