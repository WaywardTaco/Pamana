using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConvoOptionObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [HideInInspector] public int ConvoOptionIndex = -1;
    private RectTransform _rect;
    private const float _onHoverScaler = 1.02f;
    private const float _onClickScaler = 0.98f;
    private const float _unclickDelay = 0.05f;
    private const float _nextConvoDelay = 0.01f;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _rect.localScale = new Vector3(_onHoverScaler, _onHoverScaler, _onHoverScaler);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {   
        _rect.localScale = new Vector3(_onClickScaler, _onClickScaler, _onClickScaler);
        StartCoroutine(DelayedUnclick());
    }

    private IEnumerator DelayedUnclick(){
        yield return new WaitForSeconds(_unclickDelay);
        _rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(_nextConvoDelay);
        ConversationManager.Instance.NextConvoStepCallback(ConvoOptionIndex);
    }

    void Start()
    {
        _rect = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        _rect = GetComponent<RectTransform>();
    }
}
