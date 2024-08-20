using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 15;

    public AudioClip healSfx;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().HealHealth(healAmount);
            GameManager.Instance.audioSource.PlayOneShot(healSfx);
            Destroy(gameObject);
        }
    }
}
