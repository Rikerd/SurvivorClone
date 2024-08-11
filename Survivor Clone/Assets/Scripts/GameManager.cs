using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUpButtonInfo
{
    public string name;
    public UnityAction callback;
    public string description;

    public LevelUpButtonInfo(string levelUpName, UnityAction levelUpCallback, string levelUpDescription)
    {
        name = levelUpName;
        callback = levelUpCallback;
        description = levelUpDescription;
    }
}

public class GameManager : MonoBehaviour
{
    public WeaponManager weaponManager;
    public static GameManager Instance;

    public GameObject levelUpPanel;

    private int playerKillCount = 0;

    private PlayerController playerController;
    private Button[] levelUpPanelButtons;

    private LevelUpButtonInfo[] levelUpButtonInfos;

    private int temp = 0;

    // Start is called before the first frame update
    private void Start()
    {
        levelUpPanel.SetActive(false);
        levelUpPanelButtons = levelUpPanel.GetComponentsInChildren<Button>();

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        levelUpButtonInfos = new LevelUpButtonInfo[]
        {
            new LevelUpButtonInfo("Health", playerController.LevelUpPlayerHealth, ""),
            new LevelUpButtonInfo("Movement Speed", playerController.LevelUpPlayerMovementSpeed, ""),
            new LevelUpButtonInfo("Crit Chance", playerController.LevelUpPlayerCritChance, ""),
        };
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

    private void OnEnable()
    {
        // ExperienceManager
        ExperienceManager.Instance.OnLevelUp += HandleLevelUp;
    }

    private void OnDisable()
    {
        // ExperienceManager
        ExperienceManager.Instance.OnLevelUp -= HandleLevelUp;
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

    // ExperienceManager
    private void HandleLevelUp(int currentExp, int maxExp)
    {
        Time.timeScale = 0;
        levelUpPanel.SetActive(true);

        temp = 0;

        foreach (Button levelUpPanelButton in levelUpPanelButtons)
        {
            PopulateLevelUpButton(levelUpPanelButton);
        }
    }

    private void PopulateLevelUpButton(Button levelUpPanelButton)
    {
        levelUpPanelButton.onClick.RemoveAllListeners();
        bool isWeaponUpgrade = Random.Range(0, 2) == 0;

        string nameText = levelUpButtonInfos[temp].name;
        UnityAction callback = levelUpButtonInfos[temp].callback;

        if (isWeaponUpgrade)
        {
            Weapon weapon = weaponManager.GetWeaponToLevel();
            nameText = weapon.levelUpInfo.upgradeName;
            callback = weapon.LevelUpWeapon;
        }
        levelUpPanelButton.GetComponentInChildren<TMP_Text>().SetText(nameText);
        levelUpPanelButton.onClick.AddListener(callback);
        levelUpPanelButton.onClick.AddListener(CloseLevelUpPanel);

        temp++;
    }

    private void CloseLevelUpPanel()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public float GetPlayerCritChance()
    {
        return playerController.GetCritChance();
    }
}
