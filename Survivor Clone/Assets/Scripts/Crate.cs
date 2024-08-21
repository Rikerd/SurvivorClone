using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Crate : MonoBehaviour, IDamageable
{
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
        TMP_Text damageText = Instantiate(crateStat.damageText, transform.position, Quaternion.identity).GetComponent<TMP_Text>();
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
                        float radius = Random.Range(0.5f, 1.5f);
                        spread = Random.insideUnitCircle * radius;
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
