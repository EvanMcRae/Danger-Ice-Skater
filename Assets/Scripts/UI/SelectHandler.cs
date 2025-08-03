using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectHandler : MonoBehaviour, ISelectHandler, IPointerMoveHandler, IDeselectHandler
{
    public void OnDeselect(BaseEventData eventData)
    {
        ColorBlock colors = GetComponent<Selectable>().colors;
        colors.highlightedColor = colors.normalColor;
        GetComponent<Selectable>().colors = colors;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!MenuManager.tweening)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
            ColorBlock colors = GetComponent<Selectable>().colors;
            colors.highlightedColor = colors.selectedColor;
            GetComponent<Selectable>().colors = colors;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!MenuManager.pleaseNoSound)
        {
            AkUnitySoundEngine.PostEvent("HighlightUI", PauseManager.globalWwise);
        }
        else
        {
            MenuManager.pleaseNoSound = false;
        }
    }

    
}
