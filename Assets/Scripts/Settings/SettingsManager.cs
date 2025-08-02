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

    private void Start()
    {
        fullscreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);
        VSyncToggle.SetIsOnWithoutNotify(QualitySettings.vSyncCount == 1);
    }
    //PlayerPrefs.Save();//???

    public void toggleFullscreen()
    {
        //Screen.fullScreen = fullscreenToggle.isOn;
        //Screen.SetResolution(900, 900, fullscreenToggle.isOn);
        Screen.SetResolution(Display.main.systemWidth, (int)(9 / 16f * Display.main.systemWidth), fullscreenToggle.isOn);
        //PlayerPrefs.SetString("fullscreen", fullscreenToggle.isOn.ToString());
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

        AkUnitySoundEngine.SetRTPCValue("masterVolume", masterVolume.value);
        PlayerPrefs.SetFloat("volume", masterVolume.value);
    }

    public void changeMusicVolume()
    {
        AkUnitySoundEngine.SetRTPCValue("masterVolume", musicVolume.value);
        PlayerPrefs.SetFloat("volume", musicVolume.value);
    }
    public void changeEffectVolume()
    {
        AkUnitySoundEngine.SetRTPCValue("soundVolume", effectVolume.value);
        PlayerPrefs.SetFloat("volume", effectVolume.value);
    }
}
