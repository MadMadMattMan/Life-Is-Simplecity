using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class SettingsManager : MonoBehaviour
{
    private bool open;
    private bool wasMouseVisable;
    private CursorLockMode previousCursorLockMode;
    public GameObject settings;
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public InputAction interactAction;
    Resolution[] resolutions;
    private void OnEnable()
    {
        interactAction.started += OpenSettings;
        interactAction.Enable();
    }
    private void OnDisable()
    {
        interactAction.started -= OpenSettings;
        interactAction.Disable();
    }
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
    public void OpenSettings(InputAction.CallbackContext context)
    {
        if (context.started && !open)
        {
            Time.timeScale = 0;
            settings.SetActive(true);
            wasMouseVisable = Cursor.visible;
            Cursor.visible = true;
            previousCursorLockMode = Cursor.lockState;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (context.started)
        {
            Time.timeScale = 1;
            settings.SetActive(false);
            Cursor.lockState = previousCursorLockMode;
            Cursor.visible = wasMouseVisable;
        }
    }
    public void CloseSettings()
    {
        Time.timeScale = 1;
        settings.SetActive(false);
        Cursor.lockState = previousCursorLockMode;
        Cursor.visible = wasMouseVisable;
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
