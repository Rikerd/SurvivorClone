using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceMagnet : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject[] expOrbs = GameObject.FindGameObjectsWithTag("Exp Orb");

            foreach (GameObject expOrb in expOrbs)
            {
                expOrb.GetComponent<ExperiencePickUp>().StartMovement();
            }

            Destroy(gameObject);
        }
    }
}
