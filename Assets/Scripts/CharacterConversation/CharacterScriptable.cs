using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScriptable : DialogueSourceScriptable
{
    [SerializeField] public String CharacterKnownName;
    [SerializeField] public String CharacterUnknownName;
    [SerializeReference] public Sprite CharacterPortait;
}
