using UnityEngine;
using UnityEngine.UI;

public class MenuBarManager : MonoBehaviour
{
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMPro.TMP_Dropdown controlTypeDropdown;

    void Start()
    {
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        controlTypeDropdown.onValueChanged.AddListener(SetControlType);
    }

    private void OnVolumeChanged(float value)
    {
        BackgroundMusicManager.Instance.UpdateVolume(value);
    }

    private void SetControlType(int controlType)
    {
        if(controlType == 0)
        {
            CustomInput.Instance.screenInputEnabled = false;
        }
        else if(controlType == 1)
        {
            CustomInput.Instance.screenInputEnabled = true;
        }
    }

    public void PlayGame()
    {
        NavigatorManager.Instance.NavigateTo(ScreenEnum.Level);
    }

    public void Settings()
    {
        // Implement settings functionality here
        Debug.Log("Settings button clicked!");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
