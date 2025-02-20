using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterInfo", menuName = "Scriptables/CharacterInfo")]
public class CharacterInfo : ScriptableObject
{
    public enum CharacterEmotion {
        Neutral
    }
    [Serializable] public class CharacterPortraitAndEmotion {
        [SerializeReference] public Sprite CharacterPortrait;
        [SerializeField] public CharacterEmotion Emotion;
    }

    [SerializeReference] public Sprite CharacterIcon;
    [SerializeField] public List<CharacterPortraitAndEmotion> CharacterPortraits;
}
