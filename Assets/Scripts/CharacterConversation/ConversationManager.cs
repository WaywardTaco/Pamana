using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
    public enum ConvoBoxTypes {
        Basic
    }





    public static ConversationManager Instance {
        get {
            return _instance;
        }
    }
    private static ConversationManager _instance;
    void Awake()
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
