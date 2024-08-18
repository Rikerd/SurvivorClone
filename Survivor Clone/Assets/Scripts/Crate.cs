using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Crate : MonoBehaviour, IDamageable
{
    public GameObject damageTextObject;

    public BreakableStats crateStat;

    public int currentHealth { get; set; }
    public int maxHealth { get; set; }

    private List<MultipleDrops> drops;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = crateStat.maxHealth;
        maxHealth = crateStat.maxHealth;

        drops = crateStat.drops;
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

        CrateSpawnerController.Instance.DecreaseNumberOfCrates();

        Destroy(gameObject);
    }
    #endregion Health Functions
}
