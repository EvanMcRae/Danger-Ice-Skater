using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Json;

public class SettingsManager : MonoBehaviour
{
    public static Settings currentSettings;
    public const string fileName = "Settings.txt";

    // Start is called before the first frame update
    void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/" + fileName))
            LoadSettings();
        else
        {
            currentSettings = new Settings();
            SaveSettings();
        }
    }

    public static void LoadSettings()
    {
        Settings newSettings = JsonUtility.FromJson<Settings>(File.ReadAllText(Application.persistentDataPath + "/" + fileName));
        currentSettings = newSettings;
        if (currentSettings.fullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    public static void SaveSettings()
    {
        string path = Application.persistentDataPath + "/" + fileName;
        string settingsJSON = JsonUtility.ToJson(currentSettings, true);

        File.WriteAllText(path, settingsJSON);
        Debug.Log("Saved settings to: " + path);
    }
}
