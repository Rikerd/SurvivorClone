using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isDestroyOnInvisible = true;

    private int projectileDamage;
    protected float projectileSpeed;
    private bool projectileCanCrit;
    private int projectilePierceAmount;
    protected Rigidbody2D rb2d;

    private int currentProjectilePierceAmount = 0;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        // Initializes variables
        rb2d = GetComponent<Rigidbody2D>();

        currentProjectilePierceAmount = 0;
    }

    public virtual void FixedUpdate()
    {
        Vector2 move = transform.up * projectileSpeed * Time.deltaTime;
        rb2d.MovePosition(rb2d.position + move);
    }


    public void SetValues(int damage, float speed, bool canCrit, int pierceAmount)
    {
        projectileDamage = damage;
        projectileSpeed = speed;
        projectileCanCrit = canCrit;
        projectilePierceAmount = pierceAmount;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            bool isCrit = false;
            if (projectileCanCrit)
            {
                isCrit = Random.Range(0, 1f) < GameManager.Instance.GetPlayerCritChance();
            }

            int damage = projectileDamage;
            damage *= isCrit ? 2 : 1;

            collision.GetComponent<IDamageable>().DamageHealth(damage, isCrit);

            if (projectilePierceAmount != -1)
            {
                currentProjectilePierceAmount++;

                if (currentProjectilePierceAmount > projectilePierceAmount)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    void OnBecameInvisible()
    {
        if (isDestroyOnInvisible)
        {
            Destroy(gameObject);
        }
    }
}
