using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Toggle VSyncToggle;

    public Slider masterVolume;
    public Slider musicVolume;
    public Slider effectVolume;
    public static bool loadedSettings = false;

    public static float masterVolumeVal = 1;
    public static float musicVolumeVal = 0.5f;
    public static float effectVolumeVal = 0.5f;
    public int fullscreen = 1;
    public int vsync = 1;

    private void Start()
    {
        fullscreen = PlayerPrefs.GetInt("fullscreen", fullscreen);
        fullscreenToggle.isOn = fullscreen == 1;
        Screen.SetResolution(Display.main.systemWidth, (int)(9 / 16f * Display.main.systemWidth), fullscreenToggle.isOn);

        vsync = PlayerPrefs.GetInt("vsync", vsync);
        VSyncToggle.isOn = vsync == 1;
        if (VSyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        masterVolume.value = PlayerPrefs.GetFloat("masterVolume", masterVolumeVal);
        musicVolume.value = PlayerPrefs.GetFloat("musicVolume", musicVolumeVal);
        effectVolume.value = PlayerPrefs.GetFloat("soundVolume", effectVolumeVal);
        
        changeEffectVolume();
        changeMasterVolume();
        changeMusicVolume();

        loadedSettings = true;
        transform.parent.gameObject.SetActive(false);

    }

    public void toggleFullscreen()
    {
        AkUnitySoundEngine.PostEvent("SelectUI", PauseManager.globalWwise);
        Screen.SetResolution(Display.main.systemWidth, (int)(9 / 16f * Display.main.systemWidth), fullscreenToggle.isOn);
        fullscreen = fullscreenToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("fullscreen", fullscreen);
    }

    public void toggleVSync()
    {
        AkUnitySoundEngine.PostEvent("SelectUI", PauseManager.globalWwise);
        if (VSyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        PlayerPrefs.SetInt("vsync", QualitySettings.vSyncCount);
        vsync = VSyncToggle.isOn ? 1 : 0;
    }

    public void changeMasterVolume()
    {
        PlayerPrefs.SetFloat("masterVolume", masterVolume.value);
        masterVolumeVal = masterVolume.value;
        AkUnitySoundEngine.SetRTPCValue("masterVolume", 100 * PlayerPrefs.GetFloat("masterVolume"));
    }

    public void changeMusicVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume.value);
        musicVolumeVal = musicVolume.value;
        AkUnitySoundEngine.SetRTPCValue("musicVolume", 100 * PlayerPrefs.GetFloat("musicVolume"));
    }
    public void changeEffectVolume()
    {
        PlayerPrefs.SetFloat("soundVolume", effectVolume.value);
        effectVolumeVal = effectVolume.value;
        AkUnitySoundEngine.SetRTPCValue("soundVolume", 100 * PlayerPrefs.GetFloat("soundVolume"));
    }

    public void OnDestroy()
    {
        PlayerPrefs.Save();
    }

    public void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
