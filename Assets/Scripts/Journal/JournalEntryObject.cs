using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalEntryObject : DialogueObject
{
    [SerializeField] private Image _entryIconReference;
    [SerializeField] private Image _backingImage;
    public void SetJournalEntry(DialogueScriptable dialogue){
        this.SetDialogue(dialogue, true);
        JournalManager.Instance.UpdateEntryBookmarkCallback(this);
        if(dialogue == null){
            _entryIconReference.gameObject.SetActive(false);
            _backingImage.enabled = false;
            return;
        } else {
            _entryIconReference.gameObject.SetActive(true);
            _backingImage.enabled = true;
        }
        
        if(dialogue.EntrySource == null){
            Debug.LogWarning("[WARN]: Entry Source Missing");
            return;
        }
        if(dialogue.EntrySource.SourceIcon == null){
            _entryIconReference.enabled = false;
            return;
        } 
        _entryIconReference.enabled = true;
        _entryIconReference.sprite = dialogue.EntrySource.SourceIcon;
    }
}
