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
        SettingsPanel.SetActive(false);
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
        CustomInput.Instance?.ShowCursor();
        NavigatorManager.Instance.NavigateTo(ScreenEnum.Level);
    }

    public void Settings(bool open)
    {
        SettingsPanel.SetActive(open);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
