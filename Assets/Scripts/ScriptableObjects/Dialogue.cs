using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Scriptables/Dialogue/BaseDialogue", order = 999)]
public class Dialogue : ScriptableObject
{
    [SerializeField] public string DialogueText; 
}
