using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextFader : MonoBehaviour
{
    public float duration = 1.0f;

    private TMP_Text text;
    private Color textColor;
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
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - duration * Time.deltaTime);

        if (text.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
