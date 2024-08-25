using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreUpgradeButton : MonoBehaviour, IDataPersistence
{
    public StoreUpgradeStatCosts upgradeStat;
    public TMP_Text buttonText;

    public TMP_Text descriptionText;

    private int currentLevel = 0;

    public void LoadAccountData(AccountData data)
    {
        currentLevel = data.accountUpgradeTypeLevels[(int)upgradeStat.upgradeType];
        buttonText.SetText("Level " + currentLevel.ToString() + "\n" + upgradeStat.upgradeName);
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

            currentLevel++;
            buttonText.SetText("Level " + currentLevel.ToString() + "\n" + upgradeStat.upgradeName);
            DataPersistenceManager.Instance.SaveAccountData();
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }

    public void SetDescriptionText()
    {
        descriptionText.SetText(upgradeStat.upgradeDescription);
    }
}
