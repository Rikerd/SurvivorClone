using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Weapon
{
    public GameObject bombParticle;

    public AuraStats bombStat;

    public Transform bombAreaIndicator;

    public float bombSpeedRatio = 2f;

    private bool isSpawning = true;
    private Vector3 finalPosition = Vector3.zero;

    private Rigidbody2D rb2d;

    private float baseGameMoveSpeed;

    private void Start()
    {
        SetMaxCooldown(bombStat.levelStats[currentWeaponLevel].maxCooldown);

        bombAreaIndicator.localScale = new Vector2(bombStat.levelStats[currentWeaponLevel].radius, bombStat.levelStats[currentWeaponLevel].radius) * 2;

        rb2d = GetComponent<Rigidbody2D>();

        baseGameMoveSpeed = GameManager.Instance.baseGameMoveSpeed;
    }

    private void Update()
    {
        if (isSpawning)
        {
            return;
        }

        bool cooldownComplete = base.UpdateCooldownTimer();

        if (cooldownComplete)
        {
            Explode();
        }
    }

    private void FixedUpdate()
    {
        if (isSpawning)
        {
            rb2d.MovePosition(Vector3.MoveTowards(transform.position, finalPosition, baseGameMoveSpeed * bombSpeedRatio * Time.fixedDeltaTime));


            if (Vector3.Distance(transform.position, finalPosition) < 0.1f)
            {
                isSpawning = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Explode();
        }
    }

    private void Explode()
    {
        AuraLevelStats currentLevelStats = bombStat.levelStats[currentWeaponLevel];
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, currentLevelStats.radius, LayerMask.GetMask("Enemy"));
        foreach (Collider2D collider in colliders)
        {
            int damage = Random.Range(currentLevelStats.minDamage, currentLevelStats.maxDamage + 1);
            collider.GetComponent<IDamageable>().DamageHealth(damage);
        }

        GameObject particle = Instantiate(bombParticle, transform.position, Quaternion.identity);

        Destroy(particle, 1.5f);

        Destroy(gameObject);
    }

    public void SetWeaponLevel(int level)
    {
        currentWeaponLevel = level;

        bombAreaIndicator.localScale = new Vector2(bombStat.levelStats[currentWeaponLevel].radius, bombStat.levelStats[currentWeaponLevel].radius) * 2;
        SetMaxCooldown(bombStat.levelStats[currentWeaponLevel].maxCooldown);
    }

    public void StartSpawnAnimation(Vector3 spreadPosition)
    {
        isSpawning = true;
        finalPosition = spreadPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        Gizmos.DrawWireSphere(transform.position, bombStat.levelStats[currentWeaponLevel].radius);
    }
}
