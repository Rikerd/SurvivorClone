using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable, IEnemyMoveable
{
    public Stats enemyStat;
    public int currentHealth { get; set; }

    public Rigidbody2D rb2d { get; set; }
    public bool isFacingRight { get; set; }
    public Transform playerTransform { get; set; }


    // Start is called before the first frame update
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Awake()
    {
        currentHealth = enemyStat.maxHealth;
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MoveEnemy(Vector3.MoveTowards(transform.position, playerTransform.position, enemyStat.movementSpeed * Time.fixedDeltaTime));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<IDamageable>().DamageHealth(enemyStat.damage);
        }
    }

    #region Health Functions
    public void DamageHealth(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }
    }

    public void Death()
    {
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
