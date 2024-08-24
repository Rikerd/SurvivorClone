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
            case <= 2:
                transform.localScale = new Vector2(0.4f, 0.4f);
                spriteRenderer.color = new Color(0, 0.5f, 1);
                break;
            case <= 4:
                transform.localScale = new Vector2(0.43f, 0.43f);
                spriteRenderer.color = Color.green;
                break;
            case <= 5:
                transform.localScale = new Vector2(0.46f, 0.46f);
                spriteRenderer.color = Color.yellow;
                break;
            case > 5:
                transform.localScale = new Vector2(0.5f, 0.5f);
                spriteRenderer.color = Color.magenta;
                break;
        }
    }
}
