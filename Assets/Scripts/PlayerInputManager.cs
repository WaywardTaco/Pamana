using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext context){
        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if(!rayHit.collider) return;


        Interactable interactable = rayHit.collider.gameObject.GetComponent<Interactable>();
        if(interactable != null){
            if(context.started){
                Debug.Log($"Clicked interactable: {interactable.gameObject.name}");
                interactable.OnClick();
            }
            if(context.performed){
                Debug.Log($"Held on interactable: {interactable.gameObject.name}");
                interactable.OnHold();
            }
            if(context.canceled){
                Debug.Log($"Releaseed interactable: {interactable.gameObject.name}");
                interactable.OnRelease();
            }
        } else {
            if(context.started)
                Debug.Log($"Clicked object: {rayHit.collider.gameObject.name}");
            if(context.performed)
                Debug.Log($"Held on object: {rayHit.collider.gameObject.name}");
            if(context.canceled)
                Debug.Log($"Releaseed object: {rayHit.collider.gameObject.name}");
        }
    }
}
