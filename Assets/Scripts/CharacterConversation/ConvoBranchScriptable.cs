using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// [CreateAssetMenu(fileName = "New Convo Branch", menuName = "Scriptables/Convos/ConvoBranch")]
[Serializable] public class ConvoBranchScriptable
{
    public String BranchTag = "";

    [Serializable] public class ConvoStep {
        [SerializeField] public ConvoEmotion Emotion;
        [TextArea(2,3)] public string ConvoText;
        [TextArea(1,2)] public string JournalText;
        [SerializeField] public int SelfProgressSet = -1;
        [SerializeField] public bool MakesNameKnown = false;
        [SerializeReference] public ConvoStepEffectScriptable ConvoEffect = null;
        [HideInInspector] public string DialogueTag;
    }
    [Serializable] public class BranchEndOptionLink {
        [TextArea(1, 1)] public string ConvoOptionText;
        public string NextBranchTag;
        [SerializeField] public int SelfProgressSet = -1;
        [SerializeField] public bool MakesNameKnown = false;
        [SerializeReference] public ConvoStepEffectScriptable OptionEffect = null;
    }

    [SerializeField] public List<ConvoStep> ConvoSteps = new();
    [SerializeField] public List<BranchEndOptionLink> EndingOptions = new();
}

