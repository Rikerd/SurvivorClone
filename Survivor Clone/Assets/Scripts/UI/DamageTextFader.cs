using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextFader : MonoBehaviour
{
    public float duration = 1.0f;

    private TMP_Text text;
    private Color textColor;

    private float timer = 0.0f;

    private Vector3 originalPosition = Vector3.zero;

    private float fadeDirection = 1f;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;

        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime / duration;
        float lerpTime = Mathf.Lerp(1f, 0f, timer);
        float movementLerp = Mathf.Lerp(0f, 1f, timer);
        transform.localScale = new Vector2(lerpTime, lerpTime);

        Color textColor = text.color;
        textColor.a = lerpTime;
        text.color = textColor;

        transform.position = originalPosition + new Vector3(fadeDirection * movementLerp, -movementLerp, 0f);

        if (timer >= 1f)
        {
            Destroy(gameObject);
        }
    }

    public void SetFadeDirection(bool isFacingRight)
    {
        if (isFacingRight)
        {
            fadeDirection = -1f;
        }
        else
        {
            fadeDirection = 1f;
        }
    }
}
