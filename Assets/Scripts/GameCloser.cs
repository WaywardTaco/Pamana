using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameCloser : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _hoverScalar = 1.01f;
    private Vector3 _initialScale;
    void Start()
    {
        _initialScale = transform.localScale;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Application.Quit();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = _initialScale * _hoverScalar;   
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = _initialScale;
    }
}
