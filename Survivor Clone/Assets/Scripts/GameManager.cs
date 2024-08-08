using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public int nextLevelExp = 10;

    // Start is called before the first frame update
    void Start()
    {
        HUDController.hud.InitializeExpBar(nextLevelExp);
    }

    private void Awake()
    {
        gm = this;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TriggerDeathSequence()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
