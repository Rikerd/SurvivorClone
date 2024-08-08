using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private WeaponStats weaponStat;
    protected Rigidbody2D rb2d;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        // Initializes variables
        rb2d = GetComponent<Rigidbody2D>();
    }

    public virtual void FixedUpdate()
    {
        Vector2 move = transform.up * weaponStat.movementSpeed * Time.deltaTime;
        rb2d.MovePosition(rb2d.position + move);
    }

    public void SetWeaponStats(WeaponStats stat)
    {
        weaponStat = stat;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<IDamageable>().DamageHealth(weaponStat.damage);
            Destroy(gameObject);
        }
    }
}
