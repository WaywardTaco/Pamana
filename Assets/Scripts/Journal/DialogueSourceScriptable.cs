using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Source", menuName = "Scriptables/DialogueSource")]
public class DialogueSourceScriptable : ScriptableObject
{
    [SerializeReference] public Sprite SourceIcon;
}
