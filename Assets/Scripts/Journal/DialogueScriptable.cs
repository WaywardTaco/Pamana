using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Scriptable", menuName = "Scriptables/Dialogue")]
public class DialogueScriptable : ScriptableObject
{
    [SerializeReference] public DialogueSourceScriptable EntrySource;
    [SerializeField] public String DialogueTag;
    [TextArea(3, 5)]
    [SerializeField] public String DialogueText = "";
    [TextArea(1, 3)]
    [SerializeField] public String JournalText = "";

    public static DialogueScriptable CreateInstance(DialogueSourceScriptable source, String tag, String dialogueText, String journalText){
        DialogueScriptable instance = ScriptableObject.CreateInstance<DialogueScriptable>();
        
        instance.EntrySource = source;
        instance.DialogueTag = tag;
        instance.DialogueText = dialogueText;
        instance.JournalText = journalText;
        
        return instance;
    }
}
