using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreUpgradeButton : MonoBehaviour, IDataPersistence
{
    public StoreUpgradeStatCosts upgradeStat;
    public TMP_Text buttonText;
    public GameObject levelHolder;

    public TMP_Text descriptionText;

    private int currentLevel = 0;

    private Image[] levelPips;

    private void Awake()
    {
        levelPips = levelHolder.GetComponentsInChildren<Image>();
    }

    public void LoadAccountData(AccountData data)
    {
        currentLevel = data.accountUpgradeTypeLevels[(int)upgradeStat.upgradeType];

        for (int i = 0; i < currentLevel; i++)
        {
            levelPips[i].color = Color.red;
        }
        //buttonText.SetText("Cost: " + upgradeStat.coinCosts[currentLevel] + "\n" + "Level " + currentLevel.ToString() + "\n" + upgradeStat.upgradeName);
    }

    public void SaveAccountData(ref AccountData data)
    {
        data.accountUpgradeTypeLevels[(int)upgradeStat.upgradeType] = currentLevel;
    }

    public void TryUpgradeStat()
    {
        if (currentLevel >= upgradeStat.coinCosts.Count)
        {
            Debug.Log("Stat at max level");
            return;
        }

        int accountCoins = CoinManager.Instance.GetTotalCoins();

        if (accountCoins >= upgradeStat.coinCosts[currentLevel] )
        {
            CoinManager.Instance.SetTotalCoins(accountCoins - upgradeStat.coinCosts[currentLevel]);

            levelPips[currentLevel].color = Color.red;
            currentLevel++;
            //if (currentLevel >= upgradeStat.coinCosts.Count)
            //{
            //    buttonText.SetText("Max\n" + "Level " + currentLevel.ToString() + "\n" + upgradeStat.upgradeName);
            //}
            //else
            //{
            //    buttonText.SetText("Cost: " + upgradeStat.coinCosts[currentLevel] + "\n" + "Level " + currentLevel.ToString() + "\n" + upgradeStat.upgradeName);
            //}
            DataPersistenceManager.Instance.SaveAccountData();
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }

    public void SetDescriptionText()
    {
        if (currentLevel >= upgradeStat.coinCosts.Count)
        {
            descriptionText.SetText("Stat maxed out!\n" + upgradeStat.upgradeDescription);
        }
        else
        {
            descriptionText.SetText("Cost: " + upgradeStat.coinCosts[currentLevel] + "\n" + upgradeStat.upgradeDescription);
        }
    }
}
