using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagnetController : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;
    private float originalRadius = 0;

    private void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius += GameManager.Instance.GetStorePickUpRadiusAmount();
        originalRadius = circleCollider2D.radius;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin" || collision.tag == "Exp Orb")
        {
            collision.GetComponent<DrawablePickup>().StartMovement();
        }
    }

    public void UpdateRadius(float radius)
    {
        circleCollider2D.radius = originalRadius + radius;
    }
}
