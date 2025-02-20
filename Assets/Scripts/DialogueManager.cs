using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance {get; private set;}
    
    private void Awake() {
        if(Instance != null){
            Destroy(this);
            return;
        }

        Instance = this;
    }

}
