using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class JournalCanvasController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector3 _panelUpPosition;
    [SerializeField] private Vector3 _panelDownPosition;
    [SerializeField] private float _timeBeforePanelFlipUp;
    [SerializeField] private float _timeBeforePanelFlipDown;
    [SerializeField] private float _panelRevealSpeed;
    [SerializeField] private float _panelHideSpeed;
    private RectTransform _rectTransform;
    private bool _isBeingHovered = false;
    private bool _isPanelUp = false;
    private float _elapsedHoveringSpeed = 0.0f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isBeingHovered = true;
        _elapsedHoveringSpeed = 0.0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isBeingHovered = false;
        _elapsedHoveringSpeed = 0.0f;
    }

    // Start is called before the first frame update
    void Start(){
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.anchoredPosition = _panelDownPosition;
    }

    // Update is called once per frame
    void Update(){
        if((_isBeingHovered && !_isPanelUp) || (!_isBeingHovered && _isPanelUp)){
            if(HasHoverHitThreshold()) TogglePanel();
            else _elapsedHoveringSpeed += Time.deltaTime;
        }
    }

    private bool HasHoverHitThreshold(){
        return
            (_isPanelUp && _elapsedHoveringSpeed >= _timeBeforePanelFlipDown) ||
            (!_isPanelUp && _elapsedHoveringSpeed >= _timeBeforePanelFlipUp);
    }

    private void TogglePanel(){
        StopAllCoroutines();

        if(_isPanelUp) StartCoroutine(MovePanel(_panelDownPosition, false));
        else StartCoroutine(MovePanel(_panelUpPosition, true));
    }

    private IEnumerator MovePanel(Vector3 targetPosition, bool isRevealing){
        _isPanelUp = isRevealing;
        Vector3 originalPosition = _rectTransform.anchoredPosition;

        float timeProgress = 0.0f;
        float transitionTime;
        if (isRevealing) 
            transitionTime = Vector3.Distance(originalPosition, targetPosition) / _panelRevealSpeed;
        else 
            transitionTime = Vector3.Distance(originalPosition, targetPosition) / _panelHideSpeed;

        while(timeProgress < transitionTime){
            timeProgress += Time.deltaTime;
            _rectTransform.anchoredPosition = Vector3.Lerp(originalPosition, targetPosition, timeProgress/transitionTime);
            yield return new WaitForEndOfFrame();
        }

        _rectTransform.anchoredPosition = targetPosition;
    }
}
