using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChapterCorrectionIcon : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // Transform _parentAfterDrag;
    // Transform _canvas;

    private bool _justFinishedDrag = false;
    public bool DidDragJustFinish() {
        // Debug.Log($"Drag Just Finished ({this.gameObject.name}): {_justFinishedDrag}");
        if(_justFinishedDrag){
            _justFinishedDrag = false;
            return true;
        }
        else return false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // _parentAfterDrag = transform.parent;
        // transform.SetParent(_canvas);
        transform.SetAsLastSibling();
        _justFinishedDrag = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _justFinishedDrag = false;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _justFinishedDrag = true;
        // transform.SetParent(_parentAfterDrag);
    }

    void Start()
    {
        // // If the hierarchy changes this needs to change
        // GameObject Canvas = GameObject.FindGameObjectWithTag("ChapterCorrectionView");
        // if(Canvas != null)
        //     _canvas = Canvas.GetComponent<Transform>();
        // else
        //     Debug.LogWarning("[WARN]: Chapter Icon cannot find Chapter Correction View");
    }

}
