using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePickUp : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.Instance.ForceLevelUpPrompt();
            Destroy(gameObject);
        }
    }

}
