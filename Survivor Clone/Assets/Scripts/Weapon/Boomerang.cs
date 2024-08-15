using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Boomerang : Projectile
{
    public float maxDistance = 5f;
    private Vector3 originalStart = Vector3.zero;
    private Vector2 directionOfPlayer = Vector2.zero;
    private GameObject player;
    private bool maxDistanceReached = false;

    public override void Awake()
    {
        // Initializes variables
        rb2d = GetComponent<Rigidbody2D>();

        originalStart = transform.position;

        directionOfPlayer = Vector2.zero;

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
                directionOfPlayer = (player.transform.position - transform.position).normalized;
                isDestroyOnInvisible = true;
                return;
            }

            Vector2 move = transform.up * projectileSpeed * Time.deltaTime;
            rb2d.MovePosition(rb2d.position + move);
        }
        else
        {
            //rb2d.MovePosition(Vector2.MoveTowards(transform.position, player.transform.position, projectileSpeed * Time.fixedDeltaTime));
            rb2d.MovePosition(rb2d.position + directionOfPlayer * projectileSpeed * Time.fixedDeltaTime);
        }
    }
}
