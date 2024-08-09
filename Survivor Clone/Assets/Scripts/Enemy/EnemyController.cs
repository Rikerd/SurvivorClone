using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable, IEnemyMoveable
{
    public EnemyStats enemyStat;
    public GameObject damageTextObject;

    public float collisionDamageDelayTimer = 0.5f;

    public int currentHealth { get; set; }

    public Rigidbody2D rb2d { get; set; }
    public bool isFacingRight { get; set; }
    public GameObject player { get; set; }

    private bool isCollidingWithPlayer = false;
    private float currentCollisionDamageDelayTimer;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Awake()
    {
        currentHealth = enemyStat.maxHealth;
        rb2d = GetComponent<Rigidbody2D>();
        isCollidingWithPlayer = false;

        enemyStat.drops = enemyStat.drops.OrderBy(x => x.rate).ToList();
    }

    private void Update()
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

    private void FixedUpdate()
    {
        MoveEnemy(Vector3.MoveTowards(transform.position, player.transform.position, enemyStat.movementSpeed * Time.fixedDeltaTime));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isCollidingWithPlayer = true;
            currentCollisionDamageDelayTimer = 0f;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isCollidingWithPlayer = false;
        }
    }

    #region Health Functions
    public void DamageHealth(int damageAmount)
    {
        currentHealth -= damageAmount;
        GameObject damageText = Instantiate(damageTextObject, transform.position, Quaternion.identity);
        damageText.GetComponent<TMP_Text>().SetText(damageAmount.ToString());

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }
    }

    public void Death()
    {
        float rand = Random.Range(0, 100);
        foreach (Drops drop in enemyStat.drops)
        {
            if (rand <= drop.rate && drop.rate != 0)
            {
                Instantiate(drop.item, transform.position, Quaternion.identity);
                break;
            }
        }


        Destroy(gameObject);
    }
    #endregion Health Functions

    public void MoveEnemy(Vector2 movement)
    {
        rb2d.MovePosition(movement);
    }

    public void CheckForLeftOrRightFacing(Vector2 movement)
    {

    }

}
