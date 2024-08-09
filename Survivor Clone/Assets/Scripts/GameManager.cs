using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int nextLevelExp = 10;

    public static GameManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        HUDManager.Instance.InitializeExpBar(nextLevelExp);
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
}
