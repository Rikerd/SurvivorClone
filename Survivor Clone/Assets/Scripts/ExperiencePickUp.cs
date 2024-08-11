using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePickUp : MonoBehaviour
{
    public int exp = 1;
    public float movementSpeed = 5f;

    private bool isExpOrbMoving = false;
    private Rigidbody2D rb2d;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        isExpOrbMoving = false;
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isExpOrbMoving)
        {
            Vector3 movement = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.fixedDeltaTime);

            rb2d.MovePosition(movement);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ExperienceManager.Instance.AddExperience(exp);
            Destroy(gameObject);
        }
    }

    public void StartMovement()
    {
        isExpOrbMoving = true;
    }
}
