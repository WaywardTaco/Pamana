using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectableChapter : MonoBehaviour
{
    
    [Serializable] public class ItemTracker {
        [SerializeField] public ChapterCorrectionIcon IconObject;
        [SerializeField] public GameObject SlotObject;
        [HideInInspector] public GameObject IconCurrentSlot;
    }

    [SerializeField] private List<GameObject> _correctedViewObjects = new();
    [SerializeField] private List<GameObject> _incompleteViewObjects = new();
    [SerializeField] private List<ItemTracker> _chapterItems = new();
    private bool _isCompleted;

    void Update()
    {
        CheckActiveChapterIconsToSlots();
        CheckChapterCompletion();
    }

    private void CheckActiveChapterIconsToSlots(){
        foreach(ItemTracker icon in _chapterItems){
            if(!icon.IconObject.DidDragJustFinish()) continue;
            
            foreach(ItemTracker slot in _chapterItems){
                bool isOverlapping = CheckOverlap(icon.IconObject.gameObject, slot.SlotObject);
                // Debug.Log($"[DEBUG]: Icon {icon.IconObject} touching {slot.SlotObject}? {isOverlapping}");
                if(isOverlapping){
                    SnapIconToSlot(icon, slot.SlotObject);
                    break;
                } else {
                    icon.IconCurrentSlot = null;
                }
            }
        }
    }

    private void CompleteChapter(){
        _isCompleted = true;

        // TODO : Make this functional
        Debug.Log("[TODO]: Chapter Completed! Chapter Completion Graphics WIP");
    
        RefreshView();
    }

    private void CheckChapterCompletion(){
        if(_isCompleted) return;

        // Check each icon if it has matching tags, if not, returns without playing chapter completion
        foreach(ItemTracker item in _chapterItems){
            // Debug.Log($"[DEBUG]: Checking icon ({icon.gameObject.name}) on slot, match: {icon.DoItemTagsMatch()}");
            if(item.IconCurrentSlot != item.SlotObject) return;
        }
            
        CompleteChapter();
    }

    private bool CheckOverlap(GameObject icon, GameObject slot){
        RectTransform iconRect = icon.GetComponent<RectTransform>();
        RectTransform slotRect = slot.GetComponent<RectTransform>();

        if(iconRect == null){
            Debug.LogWarning("[WARN]: Icon Rect missing from icon");
            return false;
        }
        if(slotRect == null){
            Debug.LogWarning("[WARN]: Slot Rect missing from slot");
            return false;
        }
        
        if(
            iconRect.localPosition.x < slotRect.localPosition.x + slotRect.rect.width &&
            iconRect.localPosition.x + iconRect.rect.width > slotRect.localPosition.x &&
            iconRect.localPosition.y < slotRect.localPosition.y + slotRect.rect.height &&
            iconRect.localPosition.y + iconRect.rect.height > slotRect.localPosition.y
        ){
            return true;
        }

        return false;
    }

    private void SnapIconToSlot(ItemTracker icon, GameObject slot){
        icon.IconCurrentSlot = slot;
        icon.IconObject.transform.SetPositionAndRotation(
            slot.transform.position, slot.transform.rotation
        );
    }

    void OnEnable(){
        RefreshView();
    }

    private void RefreshView(){
        foreach(GameObject obj in _correctedViewObjects)
            obj.SetActive(_isCompleted);
        foreach(GameObject obj in _incompleteViewObjects)
            obj.SetActive(!_isCompleted);
    }

}
