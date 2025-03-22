using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Scriptable", menuName = "Scriptables/Dialogue")]
public class DialogueScriptable : ScriptableObject
{
    [SerializeReference] public DialogueSourceScriptable EntrySource;
    [SerializeField] public string DialogueTag;
    [TextArea(3, 5)]
    [SerializeField] public string DialogueText = "";
    [TextArea(1, 3)]
    [SerializeField] public string JournalText = "";

    public static DialogueScriptable CreateInstance(DialogueSourceScriptable source, string tag, string dialogueText, string journalText){
        DialogueScriptable instance = ScriptableObject.CreateInstance<DialogueScriptable>();
        
        instance.EntrySource = source;
        instance.DialogueTag = tag;
        instance.DialogueText = dialogueText;
        instance.JournalText = journalText;
        
        return instance;
    }
}
