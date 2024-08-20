using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePickUp : DrawablePickup
{
    private float exp = 1;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ExperienceManager.Instance.AddExperience(exp);
            GameManager.Instance.audioSource.PlayOneShot(pickUpSfx);
            Destroy(gameObject);
        }
    }

    public void SetExperienceAmount(float amount)
    {
        exp = amount;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        switch (exp)
        {
            case <= 10:
                transform.localScale = new Vector2(0.5f, 0.5f);
                spriteRenderer.color = Color.blue;
                break;
            case <= 19:
                transform.localScale = new Vector2(0.53f, 0.53f);
                spriteRenderer.color = Color.green;
                break;
            case <= 29:
                transform.localScale = new Vector2(0.56f, 0.56f);
                spriteRenderer.color = Color.yellow;
                break;
            case > 29:
                transform.localScale = new Vector2(0.6f, 0.6f);
                spriteRenderer.color = Color.magenta;
                break;
        }
    }
}
