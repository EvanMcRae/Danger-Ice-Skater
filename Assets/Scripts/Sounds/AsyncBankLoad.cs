using UnityEngine;
using AK.Wwise;
using System;

public class AsyncBankLoad : MonoBehaviour
{
    [SerializeField] private string bankName = "Main";
    [SerializeField] private string eventName = "";

    private void Awake()
    {
        AkBankManager.LoadBankAsync(bankName, OnBankLoaded);
    }

    private void OnBankLoaded(uint in_bankID, IntPtr in_InMemoryBankPtr, AKRESULT in_eLoadResult, object in_Cookie)
    {
        if (in_eLoadResult == AKRESULT.AK_Success)
        {
            Debug.Log($"Successfully loaded Wwise bank: {bankName}");
            AkUnitySoundEngine.PostEvent(eventName, PauseManager.globalWwise);
        }
        else
        {
            Debug.LogError($"Failed to load Wwise bank: {bankName} with error: {in_eLoadResult}");
        }
    }
}