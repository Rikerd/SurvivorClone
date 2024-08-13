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

    public List<Image> weaponHUDUI;

    [Header("Level Up Panel Properties")]
    public GameObject levelUpPanel;
    public List<Image> levelUpPanelIcons;
    public GameObject levelUpButtonPanel;

    private int playerKillCount = 0;

    private PlayerController playerController;
    private Button[] levelUpPanelButtons;

    private List<LevelUpButtonInfo> levelUpButtonInfos;

    private int currentLevelUpButtonIndex = 0;

    private float currentGameTime = 0f;

    private int enemyDifficulty = 0;

    private int currentWeaponHudUIIndex = 0;

    // Start is called before the first frame update
    private void Start()
    {
        levelUpPanel.SetActive(false);
        levelUpPanelButtons = levelUpButtonPanel.GetComponentsInChildren<Button>();

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
        enemyDifficulty = Mathf.FloorToInt(currentGameTime / 60);
        enemySpawnerController.UpdateSpawnPattern(enemyDifficulty);

        int miniBossPatternToSpawn = Mathf.FloorToInt(currentGameTime / 30);
        enemySpawnerController.CheckToSpawnMiniBoss(miniBossPatternToSpawn);
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

    public void ForceLevelUpPrompt()
    {
        HandleLevelUp(0, 0);
    }

    // ExperienceManager
    private void HandleLevelUp(int currentExp, int maxExp)
    {
        Time.timeScale = 0;
        levelUpPanel.SetActive(true);

        int numOfWeapons = Random.Range(1, levelUpPanelButtons.Length + 1);
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
        Image currentLevelUpPanelIcon = levelUpPanelIcons[currentLevelUpButtonIndex];
        currentLevelUpPanelIcon.sprite = null;

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
        Image currentLevelUpPanelIcon = levelUpPanelIcons[currentLevelUpButtonIndex];
        currentLevelUpPanelIcon.sprite = weapon.levelUpInfo.uiSprite;

        Button currentLevelUpButton = levelUpPanelButtons[currentLevelUpButtonIndex];
        currentLevelUpButton.onClick.RemoveAllListeners();

        string levelText = "Lv. " + (weapon.GetCurrentWeaponLevel() + 1);
        string weaponName = weapon.levelUpInfo.upgradeName;
        string weaponDescription = weapon.levelUpInfo.description;

        if (weapon.gameObject.activeSelf)
        {
            currentLevelUpButton.onClick.AddListener(weapon.LevelUpWeapon);
            levelText += " -> Lv. " + (weapon.GetCurrentWeaponLevel() + 2);
        }
        else
        {
            currentLevelUpButton.onClick.AddListener(weapon.ActivateWeapon);
        }

        string finalText = levelText + " " + weaponName + "\n" + weaponDescription;

        currentLevelUpButton.GetComponentInChildren<TMP_Text>().SetText(finalText);
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

    public void UpdateWeaponHUDUI(Sprite sprite)
    {
        weaponHUDUI[currentWeaponHudUIIndex].sprite = sprite;
        currentWeaponHudUIIndex++;
    }
}
