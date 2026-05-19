using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject settingsPanel;
    public Slider musicSlider;
    public Slider sfxSlider;

    public void OpenSettings()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayClickSound();

            musicSlider.value = AudioManager.instance.maxMusicVolume;
            sfxSlider.value = AudioManager.instance.sfxVolume;
        }

        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlayClickSound();
        settingsPanel.SetActive(false);
        PlayerPrefs.Save();
    }

    public void OnMusicSliderChanged(float value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.UpdateMusicVolume(value);
        }
    }

    public void OnSFXSliderChanged(float value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.UpdateSFXVolume(value);
        }
    }
}