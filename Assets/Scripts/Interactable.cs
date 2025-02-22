using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(){
        Debug.Log("Interactable: Click");
    }

    public void OnHold(){
        Debug.Log("Interactable: Hold");

    }

    public void OnRelease(){
        Debug.Log("Interactable: Release");

    }
}
