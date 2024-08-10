using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderData;

public class Weapon : MonoBehaviour
{
    private float currentCooldown;
    private float maxCooldown;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool UpdateCooldownTimer()
    {
        if (currentCooldown <= 0f)
        {
            currentCooldown = maxCooldown;
            return true;
        }
        else if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
        }

        return false;
    }

    protected void SetMaxCooldown(float max)
    {
        maxCooldown = max;
        currentCooldown = max;
    }
}
