using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int playerKillCount = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TriggerDeathSequence()
    {
        SceneManager.LoadScene(0);
    }

    public void IncrementKillCount()
    {
        playerKillCount++;
        HUDManager.Instance.UpdateKillCountValue(playerKillCount);
    }
}
