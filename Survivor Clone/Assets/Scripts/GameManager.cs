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
    public EnemySpawnerController enemySpawnerController;
    public static GameManager Instance;

    public GameObject levelUpPanel;

    private int playerKillCount = 0;

    private PlayerController playerController;
    private Button[] levelUpPanelButtons;

    private List<LevelUpButtonInfo> levelUpButtonInfos;

    private int currentLevelUpButtonIndex = 0;

    private float currentGameTime = 0f;

    private int enemyDifficulty = 0;

    // Start is called before the first frame update
    private void Start()
    {
        levelUpPanel.SetActive(false);
        levelUpPanelButtons = levelUpPanel.GetComponentsInChildren<Button>();

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        levelUpButtonInfos = new List<LevelUpButtonInfo>
        {
            new LevelUpButtonInfo("Health", playerController.LevelUpPlayerHealth, ""),
            new LevelUpButtonInfo("Movement Speed", playerController.LevelUpPlayerMovementSpeed, ""),
            new LevelUpButtonInfo("Crit Chance", playerController.LevelUpPlayerCritChance, ""),
        };

        currentGameTime = 0;
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
    private void Update()
    {
        currentGameTime += Time.deltaTime;
        HUDManager.Instance.UpdateTimeValue(currentGameTime);
        enemyDifficulty = Mathf.FloorToInt(currentGameTime / 20);
        enemySpawnerController.UpdateDifficulty(enemyDifficulty);
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

        int numOfWeapons = Random.Range(0, levelUpPanelButtons.Length);
        List<Weapon> weapons = weaponManager.GetWeaponsToLevel(numOfWeapons);
        currentLevelUpButtonIndex = 0;

        foreach (Weapon weapon in weapons)
        {
            PopulateLevelUpButtonWithWeapon(weapon);
            currentLevelUpButtonIndex++;
        }

        HelperFunctions.ShuffleList(ref levelUpButtonInfos);
        for (int levelUpButtonInfosIndex = 0; currentLevelUpButtonIndex < levelUpPanelButtons.Length; levelUpButtonInfosIndex++, currentLevelUpButtonIndex++)
        {
            LevelUpButtonInfo powerUpInfo = levelUpButtonInfos[levelUpButtonInfosIndex];
            PopulateLevelUpButtonWithPowerUps(powerUpInfo);
        }
    }

    private void PopulateLevelUpButtonWithPowerUps(LevelUpButtonInfo powerUpInfo)
    {
        Button currentLevelUpButton = levelUpPanelButtons[currentLevelUpButtonIndex];
        currentLevelUpButton.onClick.RemoveAllListeners();

        string nameText = powerUpInfo.name;
        UnityAction callback = powerUpInfo.callback;
        currentLevelUpButton.GetComponentInChildren<TMP_Text>().SetText(nameText);
        currentLevelUpButton.onClick.AddListener(callback);
        currentLevelUpButton.onClick.AddListener(CloseLevelUpPanel);
    }

    private void PopulateLevelUpButtonWithWeapon(Weapon weapon)
    {
        Button currentLevelUpButton = levelUpPanelButtons[currentLevelUpButtonIndex];
        currentLevelUpButton.onClick.RemoveAllListeners();

        string nameText = weapon.levelUpInfo.upgradeName;

        if (weapon.gameObject.activeSelf)
        {
            currentLevelUpButton.onClick.AddListener(weapon.LevelUpWeapon);
        }
        else
        {
            currentLevelUpButton.onClick.AddListener(weapon.ActivateWeapon);
        }

        currentLevelUpButton.GetComponentInChildren<TMP_Text>().SetText(nameText);
        currentLevelUpButton.onClick.AddListener(CloseLevelUpPanel);
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
