using UnityEngine;
using UnityEngine.EventSystems;

public class SelectHandler : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        AkUnitySoundEngine.PostEvent("HighlightUI", PauseManager.globalWwise);
    }
}
