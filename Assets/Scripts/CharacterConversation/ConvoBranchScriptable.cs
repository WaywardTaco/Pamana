using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Convo Branch", menuName = "Scriptables/Convos/ConvoBranch")]
public class ConvoBranchScriptable : ScriptableObject
{
    [Serializable] public class ConvoStep {
        [SerializeField] public ConvoEmotion Emotion;
        [TextArea(2,3)] public String ConvoText;
        [SerializeReference] public ConvoStepEffectScriptable ConvoEffect = null;
    }
    [Serializable] public class BranchEndOptionLink {
        [TextArea(1, 1)] public String ConvoOptionText;
        [SerializeReference] public ConvoBranchScriptable NextConvoOption;
        [SerializeReference] public ConvoStepEffectScriptable OptionEffect = null;
    }

    [SerializeField] public List<ConvoStep> ConvoSteps = new();
    [SerializeField] public List<BranchEndOptionLink> EndingOptions = new();
}

