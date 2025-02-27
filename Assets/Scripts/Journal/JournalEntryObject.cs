using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalEntryObject : DialogueObject
{
    [SerializeField] private Image _entryIconReference;
    public void SetJournalEntry(DialogueScriptable dialogue){
        this.SetDialogue(dialogue);
        JournalManager.Instance.UpdateEntryBookmarkCallback(this);
        if(dialogue == null){
            _entryIconReference.gameObject.SetActive(false);
            return;
        } else {
            _entryIconReference.gameObject.SetActive(true);
        }
        
        if(dialogue.EntrySource == null){
            Debug.LogWarning("[WARN]: Entry Source Missing");
            return;
        }
        if(dialogue.EntrySource.SourceIcon == null){
            Debug.LogWarning("[WARN]: Source Icon Missing");
            return;
        } 
        _entryIconReference.sprite = dialogue.EntrySource.SourceIcon;
    }
}
