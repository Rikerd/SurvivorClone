using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinPickup : DrawablePickup
{
    public bool isLargePickUp;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (isLargePickUp)
            {
                GameManager.Instance.EarnCoinByAmountWithMultiplier(10);
            }
            else
            {
                GameManager.Instance.EarnCoinByAmountWithMultiplier(1);
            }

            GameManager.Instance.audioSource.PlayOneShot(pickUpSfx);
            Destroy(gameObject);
        }
    }
}
