using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour, IDataPersistence
{
    public int totalCoins = 0;
    public TMP_Text coinText;

    public static CoinManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void LoadAccountData(AccountData data)
    {
        totalCoins = data.coins;
        coinText.SetText("Coins: " + totalCoins.ToString());
    }

    public void SaveAccountData(ref AccountData data)
    {
        data.coins = totalCoins;
    }

    public int GetTotalCoins()
    {
        return totalCoins;
    }

    public void SetTotalCoins(int amount)
    {
        totalCoins = amount;
        coinText.SetText("Coins: " + totalCoins.ToString());
    }
}
