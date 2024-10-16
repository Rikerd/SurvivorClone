using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using static UnityEngine.InputSystem.UI.VirtualMouseInput;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public StoreUpgradeStatRates storeUpgradeStatRates;

    public Texture2D cursorTexture;

    public float baseGameMoveSpeed = 3f;

    public EnemySpawnerController enemySpawnerController;
    public static GameManager Instance;

    public List<Image> weaponHUDUI;
    public List<Image> passiveHUDUI;

    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public GameObject pausePanel;

    public AudioSource audioSource;

    [Header("Level Up Panel Properties")]
    public AudioClip levelUpSfx;
    public GameObject levelUpPanel;
    public List<Image> levelUpPanelIcons;
    public GameObject levelUpButtonPanel;
    public Sprite coinFillerSprite;

    private int playerKillCount = 0;

    private PlayerController playerController;
    private Button[] levelUpPanelButtons;

    private int currentLevelUpButtonIndex = 0;

    private float currentGameTime = 0f;

    private float currentSpawnPatternEndTime;
    private float currentTimeEventSpawnTime;
    private float currentMiniBossSpawnTime;
    private float finalBossTime = 900f;
    private bool finalBossSpawned = false;

    private int currentWeaponHudUIIndex = 0;
    private int currentPassiveHudUIIndex = 0;

    private int currentCoinEarned = 0;

    private float storeDamageUpgradeMultiplier = 0f;
    private float storeMaxHealthUpgradeMultiplier = 0f;
    private int storeArmorUpgradeAmount = 0;
    private int storeProjectileUpgradeAmount = 0;
    private float storeMovementSpeedUpgradeAmount = 0;
    private float storeExperienceUpgradeMultiplier = 0;
    private int storeCoinUpgradeMultiplier = 0;
    private float storePickUpRadiusUpgradeAmount = 0;

    private PlayerInput playerInput;
    private InputAction pauseAction;

    // Start is called before the first frame update
    private void Start()
    {
        levelUpPanel.SetActive(false);
        levelUpPanelButtons = levelUpButtonPanel.GetComponentsInChildren<Button>();

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        pausePanel.SetActive(false);

        currentGameTime = 0;
        currentSpawnPatternEndTime = enemySpawnerController.GetCurrentSpawnPatternEndTimer();
        currentTimeEventSpawnTime = enemySpawnerController.GetCurrentTimeEventSpawnTimer();
        currentMiniBossSpawnTime = enemySpawnerController.GetCurrentMiniBossSpawnTimer();
        finalBossSpawned = false;

        audioSource = GetComponent<AudioSource>();

        Cursor.SetCursor(cursorTexture, new Vector2(8, 8), UnityEngine.CursorMode.Auto);
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

        playerInput = GetComponent<PlayerInput>();
        pauseAction = playerInput.actions["Pause"];
    }

    private void OnEnable()
    {
        pauseAction.Enable();
        pauseAction.started += OnPause;

        // ExperienceManager
        ExperienceManager.Instance.OnLevelUp += HandleLevelUp;
    }

    private void OnDisable()
    {
        pauseAction.Disable();
        pauseAction.started -= OnPause;

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

        if (Input.GetKeyDown(KeyCode.O))
        {
            EarnCoinByAmountWithMultiplier(100000);
        }

        currentGameTime += Time.deltaTime;
        HUDManager.Instance.UpdateTimeValue(currentGameTime);

        if (currentGameTime >= finalBossTime && !finalBossSpawned)
        {
            enemySpawnerController.SpawnFinalBoss();
            finalBossSpawned = true;
        }

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

        if (!enemySpawnerController.IsLastMiniBossSpawn() && currentGameTime >= currentMiniBossSpawnTime)
        {
            enemySpawnerController.SpawnMiniBoss();
            currentMiniBossSpawnTime = enemySpawnerController.GetCurrentMiniBossSpawnTimer();
        }
    }
    public void LoadAccountData(AccountData data)
    {
        storeDamageUpgradeMultiplier = data.accountUpgradeTypeLevels[(int)AccountData.UpgradeType.Damage] * storeUpgradeStatRates.damageMultiplierRate;
        storeMaxHealthUpgradeMultiplier = data.accountUpgradeTypeLevels[(int)AccountData.UpgradeType.MaxHealth] * storeUpgradeStatRates.healthMultiplierRate;
        storeArmorUpgradeAmount = data.accountUpgradeTypeLevels[(int)AccountData.UpgradeType.Armor] * storeUpgradeStatRates.armorRate;
        storeProjectileUpgradeAmount = data.accountUpgradeTypeLevels[(int)AccountData.UpgradeType.Projectile] * storeUpgradeStatRates.projectileRate;
        storeMovementSpeedUpgradeAmount = data.accountUpgradeTypeLevels[(int)AccountData.UpgradeType.MovementSpeed] * storeUpgradeStatRates.movementSpeedRate;
        storeExperienceUpgradeMultiplier = data.accountUpgradeTypeLevels[(int)AccountData.UpgradeType.Experience] * storeUpgradeStatRates.experienceMultiplierRate;
        storeCoinUpgradeMultiplier = data.accountUpgradeTypeLevels[(int)AccountData.UpgradeType.CoinDrop] * storeUpgradeStatRates.coinMultiplierRate + 1;
        storePickUpRadiusUpgradeAmount = data.accountUpgradeTypeLevels[(int)AccountData.UpgradeType.PickUpRadius] * storeUpgradeStatRates.pickUpRadiusRate;
    }

    public void SaveAccountData(ref AccountData data)
    {
        data.coins = data.coins + currentCoinEarned;
    }

    public void TriggerDeathSequence()
    {
        Cursor.SetCursor(null, Vector2.zero, UnityEngine.CursorMode.Auto);
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        HUDManager.Instance.SetGameOverPanelValues(playerKillCount, currentCoinEarned);
        DataPersistenceManager.Instance.SaveAccountData();
    }

    public void TriggerWinSequence()
    {
        Cursor.SetCursor(null, Vector2.zero, UnityEngine.CursorMode.Auto);
        Time.timeScale = 0;
        victoryPanel.SetActive(true);
        HUDManager.Instance.SetVictoryPanelValues(playerKillCount, currentCoinEarned);
        DataPersistenceManager.Instance.SaveAccountData();
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
        Cursor.SetCursor(null, Vector2.zero, UnityEngine.CursorMode.Auto);
        Time.timeScale = 0;
        levelUpPanel.SetActive(true);

        GameManager.Instance.audioSource.PlayOneShot(levelUpSfx);

        currentLevelUpButtonIndex = 0;

        int currentActiveWeapons = WeaponManager.Instance.GetNumOfActiveWeapons();
        int currentActivePassives = PassiveItemManager.Instance.GetNumOfActivePassives();

        bool forceWeaponsOnly = false;
        bool forcePassivesOnly = false;

        if (currentActiveWeapons >= currentActivePassives + 2)
        {
            forcePassivesOnly = true;
        }
        else if (currentActivePassives >= currentActiveWeapons + 2)
        {
            forceWeaponsOnly = true;
        }

        int numOfWeaponsFound = 0;
        if (!forcePassivesOnly)
        {
            int numOfWeaponsToFind = Random.Range(1, levelUpPanelButtons.Length + 1);
            if (forceWeaponsOnly)
            {
                numOfWeaponsToFind = 3;
            }
            List<Weapon> weapons = WeaponManager.Instance.GetWeaponsToLevel(numOfWeaponsToFind);
            numOfWeaponsFound = weapons.Count;
            foreach (Weapon weapon in weapons)
            {
                PopulateLevelUpButtonWithWeapon(weapon);
                currentLevelUpButtonIndex++;
            }
        }

        int numOfPassivesFound = 0;
        if (!forceWeaponsOnly)
        {
            int numOfPassivesToFind = levelUpPanelButtons.Length - numOfWeaponsFound;
            if (numOfPassivesToFind > 0)
            {
                List<PassiveItem> passives = PassiveItemManager.Instance.GetPassivesToLevel(numOfPassivesToFind);
                numOfPassivesFound = passives.Count;
                foreach (PassiveItem passive in passives)
                {
                    PopulateLevelUpButtonWithPassive(passive);
                    currentLevelUpButtonIndex++;
                }
            }
        }

        if (numOfPassivesFound + numOfWeaponsFound < levelUpPanelButtons.Length)
        {
            for (int i = 0; i < levelUpPanelButtons.Length - numOfPassivesFound - numOfWeaponsFound; i++)
            {
                PopulateLevelUpButtonWithFillers();
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

        int currentWeaponLevel = weapon.GetCurrentWeaponLevel();
        string levelText = "Weapon Lv. ";
        string weaponName = weapon.levelUpInfo.upgradeName;
        string weaponDescription = "";

        if (weapon.gameObject.activeSelf)
        {
            currentLevelUpButton.onClick.AddListener(weapon.LevelUpWeapon);
            levelText += (currentWeaponLevel + 2);
            weaponDescription = weapon.levelUpInfo.levelUpDescription[currentWeaponLevel + 1];
        }
        else
        {
            currentLevelUpButton.onClick.AddListener(weapon.ActivateWeapon);
            levelText += (currentWeaponLevel + 1);
            weaponDescription = weapon.levelUpInfo.levelUpDescription[currentWeaponLevel];
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

        int currentPassiveLevel = passive.GetCurrentPassiveLevel();
        string levelText = "Passive Lv. ";
        string passiveName = passive.stat.passiveName;
        string passiveDescription = "";

        if (PassiveItemManager.Instance.IsPassiveActive(passive))
        {
            currentLevelUpButton.onClick.AddListener(() => PassiveItemManager.Instance.IncreasePassiveItemLevel(passive));
            levelText += (currentPassiveLevel + 2);
            passiveDescription = passive.stat.descriptions[currentPassiveLevel + 1];
        }
        else
        {
            currentLevelUpButton.onClick.AddListener(() => PassiveItemManager.Instance.ActivatePassiveItem(passive));
            levelText += (currentPassiveLevel + 1);
            passiveDescription = passive.stat.descriptions[currentPassiveLevel];
        }

        string finalText = levelText + " " + passiveName + "\n" + passiveDescription;

        currentLevelUpButton.GetComponentInChildren<TMP_Text>().SetText(finalText);
        currentLevelUpButton.onClick.AddListener(CloseLevelUpPanel);
    }

    private void PopulateLevelUpButtonWithFillers()
    {
        Image currentLevelUpPanelIcon = levelUpPanelIcons[currentLevelUpButtonIndex];
        currentLevelUpPanelIcon.sprite = coinFillerSprite;

        Button currentLevelUpButton = levelUpPanelButtons[currentLevelUpButtonIndex];
        currentLevelUpButton.onClick.RemoveAllListeners();

        string passiveName = "Get Coins";
        string passiveDescription = "+25 coins";

        currentLevelUpButton.onClick.AddListener(() => EarnCoinByAmount(25));

        string finalText = passiveName + "\n" + passiveDescription;

        currentLevelUpButton.GetComponentInChildren<TMP_Text>().SetText(finalText);
        currentLevelUpButton.onClick.AddListener(CloseLevelUpPanel);
    }

    private void CloseLevelUpPanel()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(8, 8), UnityEngine.CursorMode.Auto);
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

    public void EarnCoinByAmount(int amount)
    {
        currentCoinEarned += amount;
        HUDManager.Instance.UpdateCoinValue(currentCoinEarned);
    }

    public void EarnCoinByAmountWithMultiplier(int amount)
    {
        int amountEarned = amount * storeCoinUpgradeMultiplier;

        PassiveItem coinPassive = PassiveItemManager.Instance.IsPassiveActiveById(PassiveItemStats.PassiveId.CoinMultiplier);
        if (coinPassive != null)
        {
            BasicPassiveItemStats coinPassiveStats = (BasicPassiveItemStats)coinPassive.stat;

            amountEarned *= (int)coinPassiveStats.stats[coinPassive.currentLevel].rateIncrease;
        }

        currentCoinEarned += amountEarned;

        HUDManager.Instance.UpdateCoinValue(currentCoinEarned);
    }

    public float GetStoreDamageMultiplier()
    {
        return storeDamageUpgradeMultiplier;
    }

    public float GetStoreMaxHealthMultiplier()
    {
        return storeMaxHealthUpgradeMultiplier;
    }

    public int GetStoreArmorAmount()
    {
        return storeArmorUpgradeAmount;
    }

    public int GetStoreProjectileAmount()
    {
        return storeProjectileUpgradeAmount;
    }

    public float GetStoreMovementSpeedAmount()
    {
        return storeMovementSpeedUpgradeAmount;
    }

    public float GetStoreExperienceMultiplier()
    {
        return storeExperienceUpgradeMultiplier;
    }

    public int GetStoreCoinMultiplier()
    {
        return storeCoinUpgradeMultiplier;
    }

    public float GetStorePickUpRadiusAmount()
    {
        return storePickUpRadiusUpgradeAmount;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        // Do not allow pausing on game over or level up
        if (gameOverPanel.activeSelf || levelUpButtonPanel.activeInHierarchy)
        {
            return;
        }

        if (pausePanel.activeSelf)
        {
            OnResumePause();
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, UnityEngine.CursorMode.Auto);
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
    }

    public void OnResumePause()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(8, 8), UnityEngine.CursorMode.Auto);
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void OnPauseEndGame()
    {
        pausePanel.SetActive(false);
        TriggerDeathSequence();
    }
}
