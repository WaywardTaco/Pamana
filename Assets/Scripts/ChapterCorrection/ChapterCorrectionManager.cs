using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterCorrectionManager : MonoBehaviour
{
    [Serializable] public class CorrectableChapter {
        [SerializeReference] public GameObject ChapterView;
        [SerializeField] public List<ChapterCorrectionIcon> AssignedChapterIcons = new();
        [SerializeField] public List<ChapterCorrectionSlot> AssignedChapterSlots = new();
    }

    [SerializeField] private List<CorrectableChapter> _listOfChapters = new();
    public int ActiveChapterIndex;
    public CorrectableChapter ActiveChapter{
        get => _listOfChapters[ActiveChapterIndex];
    }

    void Update()
    {
        CheckActiveChapterIconsToSlots();
        CheckChapterCompletion();
    }

    private void CheckActiveChapterIconsToSlots(){
        foreach(ChapterCorrectionIcon icon in ActiveChapter.AssignedChapterIcons){
            if(!icon.DidDragJustFinish()) continue;
            
            foreach(ChapterCorrectionSlot slot in ActiveChapter.AssignedChapterSlots){
                bool isOverlapping = CheckOverlap(icon, slot);
                if(isOverlapping){
                    SnapIconToSlot(icon, slot);
                    icon.AssignSlotTag(slot);
                    break;
                } else {
                    icon.AssignSlotTag();
                }
            }
        }
    }

    private void PlayChapterCompletion(CorrectableChapter chapter){
        // TODO : Make this functional
        Debug.Log("[TODO]: Chapter Completed! Chapter Completion Graphics WIP");
    }

    private void CheckChapterCompletion(){
        // Check each icon if it has matching tags, if not, returns without playing chapter completion
        foreach(ChapterCorrectionIcon icon in ActiveChapter.AssignedChapterIcons){
            // Debug.Log($"[DEBUG]: Checking icon ({icon.gameObject.name}) on slot, match: {icon.DoItemTagsMatch()}");
            if(!icon.DoItemTagsMatch()) return;
        }

        PlayChapterCompletion(ActiveChapter);
    }

    private bool CheckOverlap(ChapterCorrectionIcon icon, ChapterCorrectionSlot slot){
        RectTransform iconRect = icon.gameObject.GetComponent<RectTransform>();
        RectTransform slotRect = slot.gameObject.GetComponent<RectTransform>();

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

    private void SnapIconToSlot(ChapterCorrectionIcon icon, ChapterCorrectionSlot slot){
        icon.transform.SetPositionAndRotation(
            slot.transform.position, slot.transform.rotation
        );
    }

    public static ChapterCorrectionManager Instance {
        get {
            return _instance;
        }
    }
    private static ChapterCorrectionManager _instance;
    void Start()
    {
        if(Instance != null){
            Destroy(this);
            return;
        }

        _instance = this;
    }
    void OnDestroy()
    {
        if(Instance == this)
            _instance = null;
    }
}
