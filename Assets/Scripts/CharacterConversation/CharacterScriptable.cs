using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class CharacterScriptable : DialogueSourceScriptable {
    [Serializable] public class PortraitEmotion {
        public String PortraitFile;
        public ConvoEmotion Emotion;
    }

    public String CharacterTag;
    public String CharacterKnownName;
    public String CharacterUnknownName;
    public List<PortraitEmotion> CharacterPortaits = new();
    public List<String> ConvoStartBranchTags = new();

    public Sprite GetPortrait(ConvoEmotion emotion){
        if(CharacterPortaits.Count <= 0){
            Debug.LogWarning($"[WARN]: No portraits assigned to {CharacterTag}");
            return null;
        }
        foreach(PortraitEmotion portrait in CharacterPortaits){
            if(portrait.Emotion == emotion)
                return ConvoImporter.Instance.GetCharacterPortrait(portrait.PortraitFile);
        }

        Debug.LogWarning($"[WARN]: No {emotion} portrait for {CharacterTag} found, returned default");
        return ConvoImporter.Instance.GetCharacterPortrait(CharacterPortaits[0].PortraitFile);
    }
}
