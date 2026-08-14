using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using System.Collections.Generic;
public class SettingsManager : MonoBehaviour
{
    public GameObject settings;
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        CloseSettings();
    }
    public void OpenSettings()
    {
        settings.SetActive(true);
    }
    public void CloseSettings()
    {
        settings.SetActive(false);
    }
    public void SetVolume(float _newVolume)
    {
        audioMixer.SetFloat("Volume", _newVolume);
    }
    public void SetQuality(int _qualityIndex)
    {
        QualitySettings.SetQualityLevel(_qualityIndex);
    }
    public void SetFullscreen(bool _isFullscreen)
    {
        Screen.fullScreen = _isFullscreen;
    }
    public void SetResolution(int _resolutionIndex)
    {
        Resolution resolution = resolutions[_resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
