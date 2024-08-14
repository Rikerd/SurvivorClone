using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Crate : MonoBehaviour, IDamageable
{
    public float minSpawnTimer = 10f;
    public float maxSpawnTimer = 15f;

    public GameObject damageTextObject;

    public BreakableStats crateStat;

    public int currentHealth { get; set; }
    public int maxHealth { get; set; }

    private bool isAlive = true;

    private float currentSpawnTimer = 0f;

    private List<MultipleDrops> drops;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        currentSpawnTimer = UnityEngine.Random.Range(minSpawnTimer, maxSpawnTimer);

        currentHealth = crateStat.maxHealth;
        maxHealth = crateStat.maxHealth;

        drops = crateStat.drops;

        isAlive = true;

        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isAlive)
        {
            return;
        }

        if (currentSpawnTimer <= 0f)
        {
            gameObject.SetActive(true);
            isAlive = true;

            currentHealth = maxHealth;
            gameObject.tag = "Enemy";
            spriteRenderer.enabled = true;
            boxCollider.enabled = true;

            currentSpawnTimer = UnityEngine.Random.Range(minSpawnTimer, maxSpawnTimer);
        }
        else if (currentSpawnTimer > 0f)
        {
            currentSpawnTimer -= Time.deltaTime;
        }
    }

    #region Health Functions
    public void DamageHealth(int damageAmount, bool isCrit = false)
    {
        currentHealth -= damageAmount;
        TMP_Text damageText = Instantiate(damageTextObject, transform.position, Quaternion.identity).GetComponent<TMP_Text>();
        damageText.SetText(damageAmount.ToString());
        if (isCrit)
        {
            damageText.color = Color.red;
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
        float randDropRate = Random.Range(0, 100);

        bool dropSpawned = false;
        foreach (MultipleDrops drop in drops)
        {
            if (randDropRate <= drop.rate && drop.rate != 0)
            {
                int numToSpawn = Random.Range(drop.minDropAmount, drop.maxDropAmount);

                for (int spawnAmount = 0; spawnAmount < numToSpawn; spawnAmount++)
                {
                    Vector3 spread = Vector3.zero;

                    if (drop.spreadDrop)
                    {
                        spread = HelperFunctions.RandomUnitVector();
                    }

                    Instantiate(drop.item, transform.position + spread, Quaternion.identity);

                    dropSpawned = true;
                }
            }

            if (dropSpawned)
            {
                break;
            }
        }

        gameObject.tag = "Untagged";
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        isAlive = false;
    }
    #endregion Health Functions
}
