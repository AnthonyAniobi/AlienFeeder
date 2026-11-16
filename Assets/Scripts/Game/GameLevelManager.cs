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
    bool isPaused = false;
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
        Debug.Log("Player has died! Restarting the level...");
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

    private void OnVolumeChanged(float value)
    {
        // BackgroundMusicManager.Instance.UpdateVolume(value);
        // UpdateVolumeText();
    }

    private void Initialize()
    {
        controlsPanel.SetActive(CustomInput.Instance.screenInputEnabled);
        pausePanel.SetActive(isPaused);
        if (!CustomInput.Instance.screenInputEnabled)
        {
            CustomInput.Instance.HideCursor();
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
