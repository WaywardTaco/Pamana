using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Convo Step Effect", menuName = "Scriptables/Convos/BaseConvoStepEffect")]
public abstract class ConvoStepEffectScriptable : ScriptableObject
{
    public abstract void Effect();
}
