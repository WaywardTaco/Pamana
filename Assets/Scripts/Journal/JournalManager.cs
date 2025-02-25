using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JournalManager : MonoBehaviour
{
    [Serializable] public class DialogueBookmarkTracker {
        [SerializeReference] public DialogueScriptable Dialogue;
        [SerializeField] public bool IsBookmarked;
    }

    [SerializeReference] private Sprite _unbookmarkedIconIdle;
    [SerializeReference] private Sprite _unbookmarkedIconHover;
    [SerializeReference] private Sprite _unbookmarkedIconPressed;
    [SerializeReference] private Sprite _bookmarkedIconIdle;
    [SerializeReference] private Sprite _bookedmarkedIconHover;
    [SerializeReference] private Sprite _bookedmarkedIconPressed;

    [SerializeField] private float _unclickCheckDelay;

    [SerializeField] private List<DialogueBookmarkTracker> _dialogueReferences = new();
    [SerializeField] private Dictionary<DialogueTag, DialogueBookmarkTracker> _dialogueTags = new();

    [SerializeField] private List<DialogueObject> _allDialogueEntries = new();
    public void AddDialogueEntry(DialogueObject entry){
        _allDialogueEntries.Add(entry);
    }
    public void ClearDialogueEntries(){
        _allDialogueEntries.Clear();
    }

    private IEnumerator DelayedUnclickCheck(DialogueObject entry){
        yield return new WaitForSeconds(_unclickCheckDelay);
        UpdateEntryBookmarkCallback(entry);
    }

    public void UpdateEntryBookmarkCallback(DialogueObject entry, bool wasClick = false){
        if(wasClick){
            _dialogueTags[entry.ActiveDialogue.Tag].IsBookmarked = !_dialogueTags[entry.ActiveDialogue.Tag].IsBookmarked;
        }

        UpdateEntryBookmarkIcon(entry);
    }

    private void UpdateEntryBookmarkIcon(DialogueObject entry){
        if(_dialogueTags[entry.ActiveDialogue.Tag].IsBookmarked){
            if(entry.WasJustClicked){
                if(_bookedmarkedIconPressed == null){
                    Debug.LogWarning("[WARN]: Bookmarked Pressed Icon Missing");
                    return;
                }

                entry.SetBookmarkSprite(_bookedmarkedIconPressed);
                StartCoroutine(DelayedUnclickCheck(entry));
            } else if (entry.IsBeingHoveredOn){
                if(_bookedmarkedIconHover == null){
                    Debug.LogWarning("[WARN]: Bookmarked Hovering Icon Missing");
                    return;
                }
                
                entry.SetBookmarkSprite(_bookedmarkedIconHover);
            } else {
                if(_bookmarkedIconIdle == null){
                    Debug.LogWarning("[WARN]: Bookmarked Idle Icon Missing");
                    return;
                }
                
                entry.SetBookmarkSprite(_bookmarkedIconIdle);
            }
        } else {     
            if(entry.WasJustClicked){
                if(_unbookmarkedIconPressed == null){
                    Debug.LogWarning("[WARN]: Unbookmarked Pressed Icon Missing");
                    return;
                }
                
                entry.SetBookmarkSprite(_unbookmarkedIconPressed);
                StartCoroutine(DelayedUnclickCheck(entry));
            } else if (entry.IsBeingHoveredOn){
                if(_unbookmarkedIconHover == null){
                    Debug.LogWarning("[WARN]: Unbookmarked Hover Icon Missing");
                    return;
                }
                
                entry.SetBookmarkSprite(_unbookmarkedIconHover);
            } else {
                if(_unbookmarkedIconIdle == null){
                    Debug.LogWarning("[WARN]: Unbookmarked Idle Icon Missing");
                    return;
                }
                
                entry.SetBookmarkSprite(_unbookmarkedIconIdle);
            }
        }
    }

    private void InitializeDialogueTagDict(){
        foreach(DialogueBookmarkTracker tracker in _dialogueReferences){
            _dialogueTags.Add(tracker.Dialogue.Tag, tracker);
        }
    }

    public static JournalManager Instance {
        get {
            return _instance;
        }
    }
    private static JournalManager _instance;
    void Awake()
    {
        if(Instance != null){
            Destroy(this);
            return;
        }

        _instance = this;

        InitializeDialogueTagDict();
        
    }
    void OnDestroy()
    {
        if(Instance == this)
            _instance = null;
    }
}
