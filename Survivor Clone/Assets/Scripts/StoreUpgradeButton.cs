using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreUpgradeButton : MonoBehaviour, IDataPersistance
{
    public StoreUpgradeStats upgradeStat;
    public TMP_Text buttonText;

    private int currentLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadAccountData(AccountData data)
    {
        currentLevel = data.accountUpgradeTypeLevels[(int)upgradeStat.upgradeType];
        buttonText.SetText("Level " + currentLevel.ToString() + "\nDamage");
    }

    public void SaveAccountData(ref AccountData data)
    {
        data.accountUpgradeTypeLevels[(int)upgradeStat.upgradeType] = currentLevel;
    }

    public void TryUpgradeStat()
    {
        int accountCoins = 10000;// PlayerPrefs.GetInt("Coin Amount");

        if (accountCoins >= upgradeStat.coinCosts[currentLevel] )
        {
            //accountCoins -= upgradeStat.coinCosts[currentLevel];
            //PlayerPrefs.SetInt("Coin Amount", accountCoins);

            currentLevel++;
            buttonText.SetText("Level " + currentLevel.ToString() + "\nDamage");
            DataPersistanceManager.Instance.SaveAccountData();
        }
    }
}
