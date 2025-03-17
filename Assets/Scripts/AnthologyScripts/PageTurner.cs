using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageTurner : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool turnsToNextPage;
    public void OnPointerClick(PointerEventData eventData)
    {
        // Debug.Log("Clicked");
        if(turnsToNextPage) PageSpreadManager.Instance.NextPage();
        else PageSpreadManager.Instance.PreviousPage();
    }
}
