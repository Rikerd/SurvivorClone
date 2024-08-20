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
        Vector3 direction = (player.transform.position - transform.position).normalized;
        CheckForLeftOrRightFacing(direction);
        MoveEnemy(Vector3.MoveTowards(transform.position, player.transform.position, baseGameMoveSpeed * enemyStat.moveSpeedRatio * Time.fixedDeltaTime));
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
        float passiveDamageMultiplier = 0;
        PassiveItem damagePassive = PassiveItemManager.Instance.IsPassiveActiveById(PassiveItemStats.PassiveId.Damage);
        if (damagePassive != null)
        {
            BasicPassiveItemStats damagePassiveStats = (BasicPassiveItemStats)damagePassive.stat;

            passiveDamageMultiplier = damagePassiveStats.stats[damagePassive.currentLevel].rateIncrease;
        }

        int additionaDamageAmount = 0;
        if (passiveDamageMultiplier > 0)
        {
            if (isCrit)
            {
                damageAmount /= 2;
                additionaDamageAmount = (int)((damageAmount * passiveDamageMultiplier) + 0.5f);
                damageAmount += additionaDamageAmount;
                damageAmount *= 2;
            }
            else
            {
                additionaDamageAmount = (int)((damageAmount * passiveDamageMultiplier) + 0.5f);
                damageAmount += additionaDamageAmount;
            }

        }

        currentHealth -= damageAmount;
        TMP_Text damageText = Instantiate(enemyStat.damageText, transform.position, Quaternion.identity).GetComponent<TMP_Text>();
        damageText.SetText(damageAmount.ToString());
        if (isCrit)
        {
            damageText.color = Color.yellow;
            damageText.fontSize *= 1.2f;
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }
    }

    public void Death()
    {
        if (enemyStat.isMiniBoss)
        {
            GameObject expOrb = Instantiate(enemyStat.expOrb, transform.position, Quaternion.identity);
            expOrb.GetComponent<ExperiencePickUp>().SetExperienceAmount(enemyStat.exp);
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

        GameManager.Instance.IncrementKillCount();
        Destroy(gameObject);
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
