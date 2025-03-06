using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConversationViewUpdater : MonoBehaviour
{
    [Serializable] public class ConvoBox {
        [SerializeField] public GameObject Box;        
        [SerializeField] public ConvoEmotion Emotion;
    }

    [SerializeField] private Image _characterPortrait;
    [SerializeField] private TMP_Text _characterName;
    [SerializeField] private List<ConvoBox> _convoBoxes = new();
    [SerializeField] private List<GameObject> _choiceBoxes = new();
    private GameObject _activeConvoBox = null;
    private Dictionary<ConvoEmotion, GameObject> _convoBoxEmotions = new();

    /* DEBUG STUFF */
        // [SerializeField] private bool debugSetConvoView = false;
        // [SerializeField] private bool debugSetChoiceView = false;
        // [SerializeField] private bool debugProgressConvoStep = false;
        // [SerializeReference] private CharacterScriptable debugCharacterToSet;
        // [SerializeReference] private ConvoBranchScriptable debugConvoBranch;
        // [SerializeField] private int debugConvoStepIndex;
        // [SerializeField] private bool debugIsCharKnown = false;
    /* DEBUG STUFF END */

    public void SetConvoView(CharacterScriptable character, ConvoBranchScriptable.ConvoStep convoStep, bool isCharacterKnown){
        DisableConvoChoices();

        if(isCharacterKnown)
            _characterName.text = character.CharacterKnownName;
        else 
            _characterName.text = character.CharacterUnknownName;

        if(_activeConvoBox != null) 
            _activeConvoBox.SetActive(false);

        ConvoEmotion activeEmotion = convoStep.Emotion;

        Sprite portrait = character.GetPortrait(activeEmotion);
        if(portrait != null)
            _characterPortrait.sprite = portrait;
        else 
            Debug.LogWarning($"[WARN]: Null character portrait returned for {character.CharacterTag}");

        if(!_convoBoxEmotions.ContainsKey(activeEmotion)){
            Debug.LogWarning($"[WARN]: Convo Box {activeEmotion} is missing! Setting to default");
            activeEmotion = ConvoEmotion.Basic;
        }

        _activeConvoBox = _convoBoxEmotions[activeEmotion];
        if(_activeConvoBox == null){
            Debug.LogWarning("[WARN]: Default Basic Convo Box is missing!");
            return;
        }

        _activeConvoBox.SetActive(true);

        // Code to update ConvoBox as a Dialogue Object
        if(!_activeConvoBox.TryGetComponent<DialogueObject>(out var dialogueObject)){
            Debug.LogWarning("[WARN]: Dialogue Object component is missing from convo box");
            return;
        }
        dialogueObject.SetDialogue(
            JournalManager.Instance.GetDialogueTracker(convoStep.DialogueTag).Dialogue
        );

        // Code to refresh the text box
        if(!_activeConvoBox.TryGetComponent<HorizontalLayoutGroup>(out var horizontalLayoutGroup)){
            Debug.LogWarning("[WARN]: Horizontal Layout Group Component is missing from text box!");
            return;
        }
        Canvas.ForceUpdateCanvases();        
        horizontalLayoutGroup.enabled = false;
        horizontalLayoutGroup.enabled = true;

    }

    public void SetChoicesView(CharacterScriptable character, ConvoBranchScriptable branch, bool isCharacterKnown){
        ConvoBranchScriptable.ConvoStep activeStep = branch.ConvoSteps[branch.ConvoSteps.Count - 1];
        
        SetConvoView(character, activeStep, isCharacterKnown);

        int numberOfChoices = branch.EndingOptions.Count;
        for(int i = 0; i < numberOfChoices; i++){
            if(i >= _choiceBoxes.Count){
                Debug.LogWarning($"[WARN]: Not enough choice boxes! Trying to assign {numberOfChoices} choices, only {_choiceBoxes.Count} available");
                return;
            }

            SetChoiceBox(_choiceBoxes[i], branch.EndingOptions[i].ConvoOptionText);
        }
    }

    public void OpenDialogueView(){

    }

    public void CloseDialogueView(){

    }

    void Update()
    {
        /* Debug Stuff*/
        // if(debugProgressConvoStep){
        //     debugProgressConvoStep = false;
        //     debugConvoStepIndex++;
        // }
        // if(debugSetChoiceView){
        //     debugSetChoiceView = false;
        //     SetChoicesView(debugCharacterToSet, debugConvoBranch, debugIsCharKnown);
        // }
        // if(debugSetConvoView){
        //     debugSetConvoView = false;
        //     SetConvoView(debugCharacterToSet, debugConvoBranch.ConvoSteps[debugConvoStepIndex], debugIsCharKnown);
        // }
        /* Debug Stuff End */
    }

    void Start()
    {
        InitiateConvoBoxDict();
        InitiateChoiceBoxes();
        gameObject.SetActive(false);
    }

    private void InitiateConvoBoxDict(){
        foreach(ConvoBox box in _convoBoxes){
            _convoBoxEmotions.Add(box.Emotion, box.Box);
        }
    }

    private void InitiateChoiceBoxes(){
        int numberOfChoiceBoxes = _choiceBoxes.Count;
        for(int i = 0; i < numberOfChoiceBoxes; i++){
            if(!_choiceBoxes[i].TryGetComponent<ConvoOptionObject>(out var convoCallback)){
                Debug.LogWarning($"[WARN]: Convo Choice Box is missing click callback!");
                continue;
            }

            convoCallback.ConvoOptionIndex = i;
        }

        DisableConvoChoices();
    }

    private void DisableConvoChoices(){
        foreach(GameObject choice in _choiceBoxes)
            choice.SetActive(false);
    }

    private void SetChoiceBox(GameObject choiceBox, String text){
        choiceBox.SetActive(true);
        TMP_Text choiceText = choiceBox.GetComponentInChildren<TMP_Text>();
        if(choiceText == null){
            Debug.LogWarning("[WARN]: Text missing from choice box!");
            return;
        }
        choiceText.text = text;
        
        if(!choiceBox.TryGetComponent<HorizontalLayoutGroup>(out var _horizontalLayoutGroup)){
            Debug.LogWarning("[WARN]: Horizontal Layout Group Component is missing from choice box!");
            return;
        }
        Canvas.ForceUpdateCanvases();        
        _horizontalLayoutGroup.enabled = false;
        _horizontalLayoutGroup.enabled = true;
    }
}
