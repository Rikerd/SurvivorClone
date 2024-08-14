using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePickUp : DrawablePickup
{
    public int exp = 1;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ExperienceManager.Instance.AddExperience(exp);
            Destroy(gameObject);
        }
    }
}
