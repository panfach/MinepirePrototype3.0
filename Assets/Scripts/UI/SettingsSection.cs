using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

// -------------------- // MINEPIRE demo // -------------------- //
public class SettingsSection : MonoBehaviour
{
    public AudioMixer musicMixer; // Название не связано с музыкой, микшер отвечает все звуки
    public Slider masterVolumeSlider, musicVolumeSlider, effectsVolumeSlider, environmentVolumeSlider;
    public Toggle fullScreenToggle;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    
    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        fullScreenToggle.isOn = Screen.fullScreen;

        qualityDropdown.value = QualitySettings.GetQualityLevel();

        float musicVolume;
        musicMixer.GetFloat("master", out musicVolume);
        masterVolumeSlider.value = musicVolume + 20 * Mathf.Sin(Mathf.PI * musicVolume / 80f);
        musicMixer.GetFloat("music", out musicVolume);
        musicVolumeSlider.value = musicVolume + 20 * Mathf.Sin(Mathf.PI * musicVolume / 80f);
        musicMixer.GetFloat("effects", out musicVolume);
        effectsVolumeSlider.value = musicVolume + 20 * Mathf.Sin(Mathf.PI * musicVolume / 80f);
        musicMixer.GetFloat("environment", out musicVolume);
        environmentVolumeSlider.value = musicVolume + 20 * Mathf.Sin(Mathf.PI * musicVolume / 80f);
    }

    public void SetMasterVolume(float volume)
    {
        musicMixer.SetFloat("master", volume - 20 * Mathf.Sin(Mathf.PI * volume / 80f));
    }

    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("music", volume - 20 * Mathf.Sin(Mathf.PI * volume / 80f));
    }

    public void SetEffectsVolume(float volume)
    {
        musicMixer.SetFloat("effects", volume - 20 * Mathf.Sin(Mathf.PI * volume / 80f));
    }

    public void SetEnvironmentVolume(float volume)
    {
        musicMixer.SetFloat("environment", volume - 20 * Mathf.Sin(Mathf.PI * volume / 80f));
    }

    public void SetQiality(int quality)             // 0, 1, 2
    {
        QualitySettings.SetQualityLevel(quality);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resIndex)
    {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}
