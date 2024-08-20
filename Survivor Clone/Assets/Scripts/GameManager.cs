using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float baseGameMoveSpeed = 3f;

    public EnemySpawnerController enemySpawnerController;
    public static GameManager Instance;

    public List<Image> weaponHUDUI;
    public List<Image> passiveHUDUI;

    public GameObject gameOverPanel;

    [Header("Level Up Panel Properties")]
    public GameObject levelUpPanel;
    public List<Image> levelUpPanelIcons;
    public GameObject levelUpButtonPanel;

    private int playerKillCount = 0;

    private PlayerController playerController;
    private Button[] levelUpPanelButtons;

    private int currentLevelUpButtonIndex = 0;

    private float currentGameTime = 0f;

    private float currentSpawnPatternEndTime;
    private float currentTimeEventSpawnTime;

    private int currentWeaponHudUIIndex = 0;
    private int currentPassiveHudUIIndex = 0;

    private int currentCoinEarned = 0;


    // Start is called before the first frame update
    private void Start()
    {
        levelUpPanel.SetActive(false);
        levelUpPanelButtons = levelUpButtonPanel.GetComponentsInChildren<Button>();

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        gameOverPanel.SetActive(false);

        currentGameTime = 0;
        currentSpawnPatternEndTime = enemySpawnerController.GetCurrentSpawnPatternEndTimer();
        currentTimeEventSpawnTime = enemySpawnerController.GetCurrentTimeEventSpawnTimer();
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            ForceLevelUpPrompt();
        }

        currentGameTime += Time.deltaTime;
        HUDManager.Instance.UpdateTimeValue(currentGameTime);

        if (!enemySpawnerController.IsLastSpawnPattern() && currentGameTime > currentSpawnPatternEndTime)
        {
            enemySpawnerController.UpdateSpawnPattern();
            currentSpawnPatternEndTime = enemySpawnerController.GetCurrentSpawnPatternEndTimer();
        }

        if (!enemySpawnerController.IsLastTimeEventSpawn() && currentGameTime >= currentTimeEventSpawnTime)
        {
            enemySpawnerController.SpawnTimeEventEnemies();
            currentTimeEventSpawnTime = enemySpawnerController.GetCurrentTimeEventSpawnTimer();
        }

        //int miniBossPatternToSpawn = Mathf.FloorToInt(currentGameTime / 60);
        //enemySpawnerController.CheckToSpawnMiniBoss(miniBossPatternToSpawn);
    }

    public void TriggerDeathSequence()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void IncrementKillCount()
    {
        playerKillCount++;
        HUDManager.Instance.UpdateKillCountValue(playerKillCount);
    }

    public void ForceLevelUpPrompt()
    {
        HandleLevelUp(0, 0, 0);
    }

    // ExperienceManager
    private void HandleLevelUp(float currentExp, float maxExp, int currentLevel)
    {
        Time.timeScale = 0;
        levelUpPanel.SetActive(true);

        currentLevelUpButtonIndex = 0;

        int numOfWeapons = Random.Range(1, levelUpPanelButtons.Length + 1);
        List<Weapon> weapons = WeaponManager.Instance.GetWeaponsToLevel(numOfWeapons);
        foreach (Weapon weapon in weapons)
        {
            PopulateLevelUpButtonWithWeapon(weapon);
            currentLevelUpButtonIndex++;
        }

        int numOfPassives = levelUpPanelButtons.Length - numOfWeapons;
        if (numOfPassives > 0)
        {
            List<PassiveItem> passives = PassiveItemManager.Instance.GetPassivesToLevel(numOfPassives);
            foreach (PassiveItem passive in passives)
            {
                PopulateLevelUpButtonWithPassive(passive);
                currentLevelUpButtonIndex++;
            }
        }
    }

    private void PopulateLevelUpButtonWithWeapon(Weapon weapon)
    {
        Image currentLevelUpPanelIcon = levelUpPanelIcons[currentLevelUpButtonIndex];
        currentLevelUpPanelIcon.sprite = weapon.levelUpInfo.uiSprite;

        Button currentLevelUpButton = levelUpPanelButtons[currentLevelUpButtonIndex];
        currentLevelUpButton.onClick.RemoveAllListeners();

        string levelText = "Weapon Lv. " + (weapon.GetCurrentWeaponLevel() + 1);
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

    private void PopulateLevelUpButtonWithPassive(PassiveItem passive)
    {
        Image currentLevelUpPanelIcon = levelUpPanelIcons[currentLevelUpButtonIndex];
        currentLevelUpPanelIcon.sprite = passive.stat.uiSprite;

        Button currentLevelUpButton = levelUpPanelButtons[currentLevelUpButtonIndex];
        currentLevelUpButton.onClick.RemoveAllListeners();

        string levelText = "Passive Lv. " + (passive.GetCurrentPassiveLevel() + 1);
        string passiveName = passive.stat.passiveName;
        string passiveDescription = passive.stat.description;

        if (PassiveItemManager.Instance.IsPassiveActive(passive))
        {
            currentLevelUpButton.onClick.AddListener(() => PassiveItemManager.Instance.IncreasePassiveItemLevel(passive));
            levelText += " -> Lv. " + (passive.GetCurrentPassiveLevel() + 2);
        }
        else
        {
            currentLevelUpButton.onClick.AddListener(() => PassiveItemManager.Instance.ActivatePassiveItem(passive));
        }

        string finalText = levelText + " " + passiveName + "\n" + passiveDescription;

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

    public void UpdatePassiveHUDUI(Sprite sprite)
    {
        passiveHUDUI[currentPassiveHudUIIndex].sprite = sprite;
        currentPassiveHudUIIndex++;
    }

    public void EarnCoin()
    {
        currentCoinEarned++;
        HUDManager.Instance.UpdateCoinValue(currentCoinEarned);
    }
}
