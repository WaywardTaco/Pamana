using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConvoStarterObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private String _characterTag;
    [SerializeField] private int _convoProgressSet = -1;

    public void OnPointerClick(PointerEventData eventData){
        ConversationManager.Instance.StartConvo(_characterTag, _convoProgressSet);
    }
}
