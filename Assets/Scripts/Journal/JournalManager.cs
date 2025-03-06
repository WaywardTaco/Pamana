using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JournalManager : MonoBehaviour
{
    [Serializable] public class DialogueBookmarkTracker {
        [SerializeReference] public DialogueScriptable Dialogue;
        [SerializeField] public bool IsBookmarked = false;
    }

    [SerializeReference] private Sprite _unbookmarkedIconIdle;
    [SerializeReference] private Sprite _unbookmarkedIconHover;
    [SerializeReference] private Sprite _unbookmarkedIconPressed;
    [SerializeReference] private Sprite _bookmarkedIconIdle;
    [SerializeReference] private Sprite _bookedmarkedIconHover;
    [SerializeReference] private Sprite _bookedmarkedIconPressed;

    [SerializeField] private float _unclickCheckDelay;

    [SerializeReference] private List<JournalEntryObject> _journalEntryReferences = new();
    private List<DialogueBookmarkTracker> _dialogueReferences = new();
    private Dictionary<String, DialogueBookmarkTracker> _dialogueTags = new();
    private List<DialogueBookmarkTracker> _toDisplay = new();
    public void AddJournalEntrySlot(JournalEntryObject entry){
        _journalEntryReferences.Add(entry);
    }
    public void RemoveJournalEntrySlot(JournalEntryObject entry){
        _journalEntryReferences.Remove(entry);
    }
    public void ClearJournalEntrySlots(){
        _journalEntryReferences.Clear();
    }

    public void AddDialogueReference(DialogueScriptable dialogueScriptable){
        DialogueBookmarkTracker tracker = new();
        tracker.Dialogue = dialogueScriptable;
        _dialogueReferences.Add(tracker);
    }

    public DialogueBookmarkTracker GetDialogueTracker(String dialogueTag){
        return _dialogueTags[dialogueTag];
    }

    public void UpdateEntryBookmarkCallback(DialogueObject entry, bool wasClick = false){
        if(entry.ActiveDialogue != null && !_dialogueTags.ContainsKey(entry.ActiveDialogue.DialogueTag)){
            AddDialogueReference(entry.ActiveDialogue);
            ReinitDialogueTagDict();
        }

        if(wasClick){
            if(entry.ActiveDialogue != null){
                DialogueBookmarkTracker tracker = _dialogueTags[entry.ActiveDialogue.DialogueTag];

                if(!tracker.IsBookmarked && _toDisplay.Count >= _journalEntryReferences.Count){
                    Debug.LogWarning("[TODO]: Display Feedback that a dialogue was not able to be bookmarked cause of max count");
                } else {
                    tracker.IsBookmarked = !tracker.IsBookmarked;
                    if(tracker.IsBookmarked){
                        _toDisplay.Add(tracker);
                    } else {
                        _toDisplay.Remove(tracker);
                    }
                }
            }

            UpdateDisplayedJournalEntries();
        }

        UpdateEntryBookmarkIcon(entry);
    }

    private void UpdateDisplayedJournalEntries(){
        for(int i = 0; i < _journalEntryReferences.Count; i++){
            if(_toDisplay.Count <= i)
                _journalEntryReferences[i].SetJournalEntry(null);
            else
                _journalEntryReferences[i].SetJournalEntry(_toDisplay[i].Dialogue);
        }
    }

    private void UpdateEntryBookmarkIcon(DialogueObject entry){
        if(entry.ActiveDialogue == null){
            return;
        }

        if(_dialogueTags[entry.ActiveDialogue.DialogueTag].IsBookmarked){
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
    
    private IEnumerator DelayedUnclickCheck(DialogueObject entry){
        yield return new WaitForSeconds(_unclickCheckDelay);
        UpdateEntryBookmarkCallback(entry);
    }


    private void InitiazlizeDialogueTagDict(){
        _dialogueTags.Clear();
        ReinitDialogueTagDict();
    }

    public void ReinitDialogueTagDict(){
        foreach(DialogueBookmarkTracker tracker in _dialogueReferences){
            if(!_dialogueTags.ContainsKey(tracker.Dialogue.DialogueTag))
                _dialogueTags.Add(tracker.Dialogue.DialogueTag, tracker);
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

    }
    void Start()
    {
        
        InitiazlizeDialogueTagDict();
        UpdateDisplayedJournalEntries();
        ConversationManager.Instance.Initialize();
    }

    void OnDestroy()
    {
        if(Instance == this)
            _instance = null;
    }
}
