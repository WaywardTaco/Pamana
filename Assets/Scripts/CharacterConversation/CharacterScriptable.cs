using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Scriptables/Character")]
public class CharacterScriptable : DialogueSourceScriptable
{
    [SerializeField] public String CharacterKnownName;
    [SerializeField] public String CharacterUnknownName;
    [SerializeField] public String CharacterTag;
    [SerializeReference] public Sprite CharacterPortait;
    [SerializeReference] public List<ConvoBranchScriptable> ConvoStartBranches;
}
