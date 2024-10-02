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

    // Start is called before the first frame update
    void Start()
    {
        Vector3 spread = HelperFunctions.RandomUnitVector() * 0.1f;

        transform.position += spread;

        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime / duration;
        float lerpTime = Mathf.Lerp(1f, 0f, timer);
        transform.localScale = new Vector2(lerpTime, lerpTime);

        Color textColor = text.color;
        textColor.a = lerpTime;
        text.color = textColor;

        if (timer >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
