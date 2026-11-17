using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool IsLastLevel(int level)
    {
        // Assuming you have a total of 5 levels in the game
        int totalLevels = new AllLevelData().levels.Count; // Replace with your actual method to get total levels
        return level >= totalLevels;
    }
}
