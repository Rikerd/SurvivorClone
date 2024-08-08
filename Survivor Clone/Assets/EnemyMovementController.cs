using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EnemyMovementController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Transform player;
    public float movementSpeed = 4f;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(Vector3.MoveTowards(transform.position, player.position, movementSpeed * Time.fixedDeltaTime));
    }
}
