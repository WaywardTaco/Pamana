using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Source", menuName = "Scriptables/DialogueSource")]
[Serializable] public class DialogueSourceScriptable
{
    [SerializeReference] public Sprite SourceIcon;
}
