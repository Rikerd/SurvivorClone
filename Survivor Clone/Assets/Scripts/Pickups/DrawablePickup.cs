using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawablePickup : MonoBehaviour
{
    public float moveSpeedRatio = 2f;
    public AudioClip pickUpSfx;

    private bool isPickUpMoving = false;

    private Rigidbody2D rb2d;
    private GameObject player;

    private float baseGameMoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        isPickUpMoving = false;
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        baseGameMoveSpeed = GameManager.Instance.baseGameMoveSpeed;
    }

    void FixedUpdate()
    {
        if (isPickUpMoving)
        {
            Vector3 movement = Vector3.MoveTowards(transform.position, player.transform.position, baseGameMoveSpeed * moveSpeedRatio * Time.fixedDeltaTime);

            rb2d.MovePosition(movement);
        }
    }

    public void StartMovement()
    {
        isPickUpMoving = true;
    }
}
