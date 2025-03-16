using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PageSpreadManager : MonoBehaviour 
{
    [SerializeField] private int _startPage;
    [SerializeField] private float _flipTime;
    // [SerializeField] private bool _debugNext;
    // [SerializeField] private bool _debugPrev;
    [SerializeField] private List<GameObject> _pageSpreads = new();
    private int _currentSpreadNumber = -1;
    private bool _isTransitioning = false;

    private Dictionary<GameObject, int> _spreadPagesDict = new();

    public void NextPage(){
        if(_isTransitioning) return;
        if(_currentSpreadNumber + 1 >= _pageSpreads.Count) return;
        StartCoroutine(FlipPage(true));
    }

    public void PreviousPage(){
        if(_isTransitioning) return;
        if(_currentSpreadNumber - 1 < 0) return;
        StartCoroutine(FlipPage(false));
    }

    public bool IsSpreadOpen(GameObject spread){
        if(!_spreadPagesDict.ContainsKey(spread)) return false;
        return _spreadPagesDict[spread] == _currentSpreadNumber;
    }

    private IEnumerator FlipPage(bool isGoingNext){
        int otherSpreadIndex;
        if(isGoingNext) otherSpreadIndex = _currentSpreadNumber + 1;
        else otherSpreadIndex = _currentSpreadNumber - 1;

        string currentSpreadAnimTriggerName, otherSpreadAnimTriggerName;
        if(isGoingNext){
            currentSpreadAnimTriggerName = "FlipNext";
            otherSpreadAnimTriggerName = "FlipFromLast";
        } else {
            currentSpreadAnimTriggerName = "FlipBack";
            otherSpreadAnimTriggerName = "FlipFromNext";
        }

        GameObject currentSpread = _pageSpreads[_currentSpreadNumber];
        GameObject otherSpread = _pageSpreads[otherSpreadIndex];
        if(currentSpread == null || otherSpread == null) yield return null;

        Animator currentAnimator = currentSpread.GetComponent<Animator>();
        Animator otherAnimator = otherSpread.GetComponent<Animator>();
        if(currentAnimator == null || otherAnimator == null) yield return null;

        _isTransitioning = true;

        currentSpread.transform.SetAsLastSibling();
        otherSpread.SetActive(true);
        currentAnimator.SetTrigger(currentSpreadAnimTriggerName);
        yield return new WaitForSeconds(_flipTime / 2.0f);
        currentSpread.SetActive(true);

        otherSpread.transform.SetAsLastSibling();
        otherAnimator.SetTrigger(otherSpreadAnimTriggerName);
        yield return new WaitForSeconds(_flipTime / 2.0f);
        currentAnimator.SetTrigger("Reset");
        currentSpread.SetActive(false);

        _isTransitioning = false;
        if(isGoingNext) _currentSpreadNumber++;
        else _currentSpreadNumber--;
    }

    void Update(){
        // if(_debugNext){
        //     _debugNext = false;
        //     NextPage();
        // }
        // if(_debugPrev){
        //     _debugPrev = false;
        //     PreviousPage();
        // }
    }

    private void InitSpreadDict(){
        for(int i = 0; i < _pageSpreads.Count; i++){
            _spreadPagesDict.Add(_pageSpreads[i], i);
        }
    }

    private void InitSetup(){
        _currentSpreadNumber = _startPage;

        for(int i = 0; i < _pageSpreads.Count; i++){
            if(i == _startPage) _pageSpreads[i].SetActive(true);
            else _pageSpreads[i].SetActive(false);
        }
    }

    public static PageSpreadManager Instance { get; private set;}
    void Start()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);

        InitSpreadDict();
        InitSetup();
    }

    void ODestroy()
    {
        if(Instance == this) Instance = null;
    }
}