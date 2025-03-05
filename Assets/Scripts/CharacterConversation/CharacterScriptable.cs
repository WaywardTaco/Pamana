using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Scriptables/Character")]
public class CharacterScriptable : DialogueSourceScriptable
{
    [Serializable] public class PortraitEmotion {
        [SerializeReference] public Sprite PortraitSprite;
        [SerializeField] public ConvoEmotion Emotion;
    }

    [SerializeField] public String CharacterKnownName;
    [SerializeField] public String CharacterUnknownName;
    [SerializeField] public String CharacterTag;
    [SerializeField] private List<PortraitEmotion> CharacterPortaits = new();
    [SerializeReference] public List<ConvoBranchScriptable> ConvoStartBranches = new();

    public Sprite GetPortrait(ConvoEmotion emotion){
        if(CharacterPortaits.Count <= 0){
            Debug.LogWarning($"[WARN]: No portraits assigned to {CharacterTag}");
            return null;
        }

        foreach(PortraitEmotion portrait in CharacterPortaits){
            if(portrait.Emotion == emotion)
                return portrait.PortraitSprite;
        }

        Debug.LogWarning($"[WARN]: No {emotion} portrait for {CharacterTag} found, returned default");
        return CharacterPortaits[0].PortraitSprite;
    }
}
