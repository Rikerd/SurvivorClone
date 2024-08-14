using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : DrawablePickup
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.Instance.EarnCoin();

            Destroy(gameObject);
        }
    }
}
