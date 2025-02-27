using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected TMP_Text _dialogueText;
    [SerializeField] protected Image _bookmark;

    [SerializeField] protected DialogueScriptable _activeDialogueEntry = null;
    public DialogueScriptable ActiveDialogue {
        get { return _activeDialogueEntry; }
    }

    private bool _isBeingHoveredOn = false;
    public bool IsBeingHoveredOn {
        get { return _isBeingHoveredOn;}
    }

    private bool _wasJustClicked = false;
    public bool WasJustClicked {
        get {
            if(_wasJustClicked){
                _wasJustClicked = false;
                return true;
            } else 
                return false;
        }
    }

    void Start()
    {
        if(_activeDialogueEntry != null)
            SetDialogue(_activeDialogueEntry);
    }

    protected virtual void SetDialogue(DialogueScriptable dialogueEntry){
        _activeDialogueEntry = dialogueEntry;
        if(dialogueEntry == null){
            _dialogueText.text = "";
            _bookmark.gameObject.SetActive(false);
            return;
        } 
        _bookmark.gameObject.SetActive(true);
        _dialogueText.text = _activeDialogueEntry.DialogueText;
    }

    public void SetBookmarkSprite(Sprite sprite, bool deactivate = false){
        _bookmark.sprite = sprite;
        _bookmark.gameObject.SetActive(!deactivate);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isBeingHoveredOn = true;
        // Debug.Log("[Debug]: Hover On Dialogue Bookmark");
        JournalManager.Instance.UpdateEntryBookmarkCallback(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isBeingHoveredOn = false;
        // Debug.Log("[Debug]: Hover Off Dialogue Bookmark");
        JournalManager.Instance.UpdateEntryBookmarkCallback(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _wasJustClicked = true;
        // Debug.Log("[Debug]: Clicked Dialogue To Bookmark");
        JournalManager.Instance.UpdateEntryBookmarkCallback(this, true);
    }
}
