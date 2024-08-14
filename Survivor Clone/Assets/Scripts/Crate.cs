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

    private List<Drops> drops;

    // Start is called before the first frame update
    void Start()
    {
        currentSpawnTimer = UnityEngine.Random.Range(minSpawnTimer, maxSpawnTimer);

        currentHealth = crateStat.maxHealth;
        maxHealth = crateStat.maxHealth;

        drops = crateStat.drops;

        isAlive = true;
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
        float rand = Random.Range(0, 100);
        foreach (Drops drop in drops)
        {
            if (rand <= drop.rate && drop.rate != 0)
            {
                Instantiate(drop.item, transform.position, Quaternion.identity);
                break;
            }
        }

        gameObject.SetActive(false);
        isAlive = false;
    }
    #endregion Health Functions
}
