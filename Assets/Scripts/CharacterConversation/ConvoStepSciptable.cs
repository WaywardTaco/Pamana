using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Convo Step", menuName = "Scriptables/ConvoStep")]
public class ConvoStepSciptable : ScriptableObject
{
    public class ConvoOptionLink {
        [TextArea(1, 1)] public String ConvoOptionText;
        [SerializeReference] public ConvoStepSciptable NextConvoOption;
    }

    [SerializeReference] public CharacterScriptable Speaker;
    [SerializeField] public ConversationManager.ConvoBoxTypes Emotion;
    [TextArea(2,3)] public String ConvoText;
    [SerializeReference] public List<ConvoOptionLink> ConvoOptionLinks = new();
    
}
