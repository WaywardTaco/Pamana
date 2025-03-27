using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConvoStarterObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _characterTag;
    [SerializeField] private float _hoverScaler = 1.01f;
    [SerializeField] private int _convoProgressSet = -1;

    public void OnPointerClick(PointerEventData eventData){
        ConversationManager.Instance.StartConvo(_characterTag, _convoProgressSet);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(_hoverScaler, _hoverScaler, _hoverScaler);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
