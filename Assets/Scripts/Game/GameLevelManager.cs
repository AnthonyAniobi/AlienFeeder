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
    public Sprite pauseSprite;
    public Sprite playSprite;

    [Header("UI Elements")]
    public GameObject joystickPanel;
    public GameObject pausePanel;
    [SerializeField] private Image pauseImage;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private Slider volumeSlider;
    bool isPaused = false;

    
    
    


    // 
    private GameObject playerInstance;
    private List<GameObject> enemyInstances = new List<GameObject>();
    private List<GameObject> edibleInstances = new List<GameObject>();

    void Start()
    {
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        Initialize();
        StartLevel(1);
    }
    void OnDestroy()
    {
        volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
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
    }

    public void StartLevel(int levelNumber)
    {
        
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
        }
        else
        {
            CustomInput.Instance.screenInputEnabled = true;
            controlsPanel.SetActive(true);
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
        }
    }
}
