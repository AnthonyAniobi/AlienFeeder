using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigatorManager : MonoBehaviour
{
    public static NavigatorManager Instance { get; private set; }
    
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

    public void NavigateTo(ScreenEnum screen)
    {
        SceneManager.LoadScene((int)screen);
    }

}


public enum ScreenEnum
{
    MainMenu = 0,
    Level = 1,
}