using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Projectile
{
    public float maxDistance = 5f;
    private Vector3 originalStart = Vector3.zero;
    private GameObject player;
    private bool maxDistanceReached = false;

    public override void Awake()
    {
        // Initializes variables
        rb2d = GetComponent<Rigidbody2D>();

        originalStart = transform.position;

        maxDistanceReached = false;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void FixedUpdate()
    {
        if (!maxDistanceReached)
        {
            float distance = Vector3.Distance(originalStart, transform.position);

            if (distance >= maxDistance)
            {
                maxDistanceReached = true;
                return;
            }

            Vector2 move = transform.up * projectileSpeed * Time.deltaTime;
            rb2d.MovePosition(rb2d.position + move);
        }
        else
        {
            rb2d.MovePosition(Vector2.MoveTowards(transform.position, player.transform.position, projectileSpeed * Time.fixedDeltaTime));

            float playerDistance = Vector3.Distance(player.transform.position, transform.position);

            if (playerDistance < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
