using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConversationViewUpdater : MonoBehaviour
{
    [Serializable] public class ConvoBox {
        [SerializeField] public GameObject Box;
        [SerializeField] public ConvoEmotion Emotion;
    }

    [SerializeField] private Image _characterPortrait;
    [SerializeField] private TMP_Text _characterName;
    [SerializeField] private List<ConvoBox> _convoBoxes = new();
    private Dictionary<ConvoEmotion, GameObject> _convoBoxEmotions = new();
    
    public void SetConvoView(CharacterScriptable character, ConvoBranchScriptable.ConvoStep convoStep, bool isCharacterKnown){
        
    }

    public void SetChoicesView(CharacterScriptable character, ConvoBranchScriptable branch, bool isCharacterKnown){

    }

    public void OpenDialogueView(){

    }

    public void CloseDialogueView(){

    }

    void Start()
    {
        
    }

    private void InitiateConvoBoxDict(){
        foreach(ConvoBox box in _convoBoxes){
            _convoBoxEmotions.Add(box.Emotion, box.Box);
        }
    }
}
