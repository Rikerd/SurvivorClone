using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpController : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Drawable Pickup")
        {
            collision.GetComponent<DrawablePickup>().StartMovement();
        }
    }
}
