using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawablePickup : MonoBehaviour
{
    public float movementSpeed = 5f;

    private bool isPickUpMoving = false;

    private Rigidbody2D rb2d;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        isPickUpMoving = false;
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        if (isPickUpMoving)
        {
            Vector3 movement = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.fixedDeltaTime);

            rb2d.MovePosition(movement);
        }
    }

    public void StartMovement()
    {
        isPickUpMoving = true;
    }
}
