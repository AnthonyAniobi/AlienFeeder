using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelManager : MonoBehaviour
{
    [Header("Prefabs and Sprites")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject ediblePrefab;
    public GameObject exitPortalPrefab;
    public Sprite pauseSprite;
    public Sprite playSprite;

    [Header("UI Elements")]
    public GameObject joystickPanel;
    public GameObject pausePanel;
    [SerializeField] private Image pauseImage;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMPro.TMP_Dropdown controlTypeDropdown;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public GameObject losePanel;
    public GameObject winPanel;


    // internal state variables
    internal bool isPaused = false;
    internal int level = 0;


    // 
    private GameObject playerInstance;
    private List<GameObject> enemyInstances = new List<GameObject>();
    private List<GameObject> edibleInstances = new List<GameObject>();

    void Start()
    {
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        controlTypeDropdown.onValueChanged.AddListener(SetControlType);
        Initialize();
        StartLevel(1);
    }
    void OnDestroy()
    {
        volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
    }

    void Update()
    {
        if (CustomInput.Instance.escape)
        {
            TogglePauseMenu();
        }
    }

    public void OnPlayerDeath()
    {
        gameOverPanel.SetActive(true);
        losePanel.SetActive(true);
        winPanel.SetActive(false);
    }

    public void OnPlayerVictory()
    {
        GameLevelData currentLevelData = new AllLevelData().levels[level];
        Instantiate(
            exitPortalPrefab, 
            currentLevelData.playerExitPosition, 
            Quaternion.identity
        );
    }

    public void OnPlayerEnterPortal()
    {
        gameOverPanel.SetActive(true);
        losePanel.SetActive(false);
        winPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game when the player wins
    }

    private void OnVolumeChanged(float value)
    {
        // BackgroundMusicManager.Instance.UpdateVolume(value);
        // UpdateVolumeText();
    }

    private void Initialize()
    {
        controlsPanel.SetActive(CustomInput.Instance.screenInputEnabled);
        pausePanel.SetActive(isPaused);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        if (!CustomInput.Instance.screenInputEnabled)
        {
            CustomInput.Instance.HideCursor();
        }
        else
        {
            CustomInput.Instance.ShowCursor();
        }
        
    }

    public void StartLevel(int levelNumber)
    {
        level = levelNumber;
        // Clear previous level instances
        if (playerInstance != null)
        {
            Destroy(playerInstance);
        }
        foreach (var enemy in enemyInstances)
        {
            Destroy(enemy);
        }
        enemyInstances.Clear();
        foreach (var edible in edibleInstances)
        {
            Destroy(edible);
        }
        edibleInstances.Clear();
        //
        GameLevelData levelData = new AllLevelData().levels[levelNumber];
        playerInstance = Instantiate(
            playerPrefab, 
            levelData.playerStartPosition, Quaternion.identity
        );
        playerInstance.GetComponentInChildren<MimicType>().Initialize(
            levelData.mimicType, 
            levelData.totalBatteriesToCollect
        );
        foreach (var enemyData in levelData.enemies)
        {
            GameObject enemyInstance = Instantiate(
                enemyPrefab, 
                enemyData.position, Quaternion.identity
            );
            enemyInstances.Add(enemyInstance);
        }
        foreach (var edibleData in levelData.edibles)
        {
            GameObject edibleInstance = Instantiate(
                ediblePrefab, 
                edibleData.position, Quaternion.identity
            );
            edibleInstances.Add(edibleInstance);
        }
        Time.timeScale = 1f; // Reset time scale in case it was paused
    }


    //
    public void ToggleOnScreenControls()
    {
        if(CustomInput.Instance.screenInputEnabled)
        {
            CustomInput.Instance.screenInputEnabled = false;
            controlsPanel.SetActive(false);
            CustomInput.Instance.HideCursor();
        }
        else
        {
            CustomInput.Instance.screenInputEnabled = true;
            controlsPanel.SetActive(true);
            CustomInput.Instance.ShowCursor();
        }
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
            if (pauseImage != null)
            {
                pauseImage.sprite = isPaused ? playSprite : pauseSprite;
            }
            Time.timeScale = isPaused ? 0f : 1f;
            if (isPaused)
            {
                CustomInput.Instance.ShowCursor();
            }
            else
            {
                // is playing game
                if(CustomInput.Instance.screenInputEnabled)
                {
                    CustomInput.Instance.ShowCursor();
                }
                else
                {
                    CustomInput.Instance.HideCursor();
                }
            }
            
        }
    }

    // 
    public void MainMenu()
    {
        NavigatorManager.Instance.NavigateTo(ScreenEnum.MainMenu);
    }

    public void NextLevel()
    {
        StartLevel(level + 1);
    }

    private void SetControlType(int controlType)
    {
        if(controlType == 0)
        {
            CustomInput.Instance.screenInputEnabled = false;
            controlsPanel.SetActive(false);
        }
        else if(controlType == 1)
        {
            CustomInput.Instance.screenInputEnabled = true;
            controlsPanel.SetActive(true);
        }
    }
}
