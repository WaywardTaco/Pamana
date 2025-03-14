using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConvoOptionObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [HideInInspector] public int ConvoOptionIndex = -1;
    public void OnPointerEnter(PointerEventData eventData)
    {
        // TODO: Change the look of the option when being hovered on
        Debug.Log("[TODO]: Change the look of the option when being hovered on");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO: Change the look of the option when being hovered off
        Debug.Log("[TODO]: Change the look of the option when being hovered off");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO: Change the look of the option when clicked
        Debug.Log("[TODO]: Change the look of the option when clicked");

        ConversationManager.Instance.NextConvoStepCallback(ConvoOptionIndex);
    }
}
