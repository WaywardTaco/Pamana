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
            
        JournalManager.Instance.UpdateEntryBookmarkCallback(this);
    }

    void OnEnable()
    {
        // JournalManager.Instance.UpdateEntryBookmarkCallback(this);   
    }

    public virtual void SetDialogue(DialogueScriptable dialogueEntry, bool isJournalEntry = false){
        _activeDialogueEntry = dialogueEntry;
        if(dialogueEntry == null){
            _dialogueText.text = "";
            _bookmark.gameObject.SetActive(false);
            return;
        } 
        _bookmark.gameObject.SetActive(true);
        if(isJournalEntry && _activeDialogueEntry.JournalText != null && !_activeDialogueEntry.JournalText.Equals(""))
            _dialogueText.text = _activeDialogueEntry.JournalText;
        else 
            _dialogueText.text = _activeDialogueEntry.DialogueText;
    }

    public void SetBookmarkSprite(Sprite sprite, float scalar, bool deactivate = false){
        _bookmark.sprite = sprite;
        _bookmark.gameObject.SetActive(!deactivate);

        if(_bookmark.gameObject.TryGetComponent<RectTransform>(out var rectTransform)){
            rectTransform.localScale = new Vector3(scalar, scalar, scalar);
        }
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
