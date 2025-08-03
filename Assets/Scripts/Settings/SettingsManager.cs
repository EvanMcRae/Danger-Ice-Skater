using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Json;
using UnityEngine.UI;
using UnityEditor;

public class SettingsManager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Toggle VSyncToggle;

    public Slider masterVolume;
    public Slider musicVolume;
    public Slider effectVolume;
    public static bool loadedSettings = false;

    private void Start()
    {
        if (!loadedSettings)
        {
            int i = PlayerPrefs.GetInt("firstLoaded");
            if (i == 0)
            {
                PlayerPrefs.SetInt("firstLoaded", 1);
                PlayerPrefs.SetFloat("masterVolume", 1);
                PlayerPrefs.SetFloat("musicVolume", 0.5f);
                PlayerPrefs.SetFloat("soundVolume", 0.5f);
                PlayerPrefs.SetInt("vsync", 1);
                PlayerPrefs.SetInt("fullscreen", 1);
            }

            fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen") == 1;
            toggleFullscreen();
            VSyncToggle.isOn = PlayerPrefs.GetInt("vsync") == 1;
            toggleVSync();

            masterVolume.value = PlayerPrefs.GetFloat("masterVolume");
            musicVolume.value = PlayerPrefs.GetFloat("musicVolume");
            effectVolume.value = PlayerPrefs.GetFloat("soundVolume");

            AkUnitySoundEngine.SetRTPCValue("masterVolume", 100 * PlayerPrefs.GetFloat("masterVolume"));
            AkUnitySoundEngine.SetRTPCValue("musicVolume", 100 * PlayerPrefs.GetFloat("musicVolume"));
            AkUnitySoundEngine.SetRTPCValue("soundVolume", 100 * PlayerPrefs.GetFloat("soundVolume"));

            loadedSettings = true;
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void toggleFullscreen()
    {
        Screen.SetResolution(Display.main.systemWidth, (int)(9 / 16f * Display.main.systemWidth), fullscreenToggle.isOn);
    }

    public void toggleVSync()
    {
        if (VSyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        
        PlayerPrefs.SetInt("vsync", QualitySettings.vSyncCount);
    }

    public void changeMasterVolume()
    {
        PlayerPrefs.SetFloat("masterVolume", masterVolume.value);
        AkUnitySoundEngine.SetRTPCValue("masterVolume", 100 * PlayerPrefs.GetFloat("masterVolume"));
    }

    public void changeMusicVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume.value);
        AkUnitySoundEngine.SetRTPCValue("musicVolume", 100 * PlayerPrefs.GetFloat("musicVolume"));
    }
    public void changeEffectVolume()
    {
        PlayerPrefs.SetFloat("soundVolume", effectVolume.value);
        AkUnitySoundEngine.SetRTPCValue("soundVolume", 100 * PlayerPrefs.GetFloat("soundVolume"));
    }
}
