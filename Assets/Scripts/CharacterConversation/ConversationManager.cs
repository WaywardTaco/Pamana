using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.SearchService;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;

public class ConversationManager : MonoBehaviour
{
    [Serializable] public class CharacterConvoTracker {
        [SerializeReference] public CharacterScriptable Character;
        [SerializeField] public int ConvoIndex;
        [SerializeField] public bool isCharacterKnown = false;
    }

    [SerializeReference] private List<CharacterConvoTracker> _characterConvoProgress = new();
    private Dictionary<String, CharacterConvoTracker> _characters = new();

    [SerializeField] private ConversationViewUpdater _viewUpdater;

    private ConvoBranchScriptable _currentConvo;
    private String _currentCharacter;
    private int _currentConvoStepIndex = -1;

    /// <summary>
    /// Call this to start a conversation based on tracked progress
    /// </summary>
    /// <param name="characterTag">Insert the tag of the character whose convo to start</param>
    public void StartConvo(String characterTag){
        if(_viewUpdater == null){
            Debug.LogWarning("[WARN]: Conversation View Updater Missing");
            return;
        }
        if(!_characters.ContainsKey(characterTag)){
            Debug.LogWarning($"[WARN]: Unknown Character Tag: {characterTag}");
            return;
        }

        CharacterConvoTracker tracker = _characters[characterTag];
        if(tracker.ConvoIndex == -1){
            Debug.Log("[DEBUG]: Conversation Disabled");
            return;
        }
        if(tracker.ConvoIndex < -1 || tracker.ConvoIndex >= tracker.Character.ConvoStartBranches.Count){
            Debug.Log("[DEBUG]: Conversation Index outside of range of convo start branches");
            return;
        }

        // Sets the current convo based on detected progress and the selected character
        _currentConvo = tracker.Character.ConvoStartBranches[tracker.ConvoIndex];
        _currentConvoStepIndex = 0;
        _currentCharacter = characterTag;
        _viewUpdater.OpenDialogueView();

        LoadConvoStep();
    }
    
    private void CloseConvo(){
        _currentConvo = null;
        _currentConvoStepIndex = -1;
        _currentCharacter = null;

        // TODO: Proper clean up of convo
        Debug.Log("[TODO]: Do proper clean up of conversation");
        
        _viewUpdater.CloseDialogueView();
    }

    /// <summary>
    /// A callback function for when a conversation step is proceeded (ie. call this when the next convo step needs to be called)
    /// </summary>
    /// <param name="choiceIndex">Set a value for which choice is chosen among dialogue options, default value = -1 (no choice)</param>
    public void NextConvoStepCallback(int choiceIndex = -1){
        ConvoBranchScriptable.ConvoStep convoStep = GetCurrentConvoStep();
        if(convoStep == null){
            Debug.LogWarning("[WARN]: Trying to progress convo without an active convo step");
            CloseConvo();
            return;
        }

        // Activates the last step's step effect
        if(convoStep.ConvoEffect != null)
            convoStep.ConvoEffect.Effect();

        // Progresses the convo (going to the next step, changing branch, or ending the convo)
        
        if(choiceIndex != -1){
            // Logic for changing branches
            if(_currentConvo == null){
                Debug.LogWarning("[WARN]: Trying to progress convo with no active convo");
                CloseConvo();
                return;
            }
            if(choiceIndex < -1 || _currentConvo.EndingOptions.Count <= choiceIndex){
                Debug.LogWarning("[WARN]: Accessing out of bounds choice");
                CloseConvo();
                return;
            }

            // Change branch
            _currentConvo = _currentConvo.EndingOptions[choiceIndex].NextConvoOption;
            _currentConvoStepIndex = 0;

            // Activate branch choice effect
            if(_currentConvo.EndingOptions[choiceIndex].OptionEffect != null)
                _currentConvo.EndingOptions[choiceIndex].OptionEffect.Effect();
        } else {
            // Logic for progressing to next step or ending convo

            // Closes convo if we are on the last convo item and no choice is selected
            if(_currentConvoStepIndex + 1 >= _currentConvo.ConvoSteps.Count){
                CloseConvo();
                return;
            }

            // Goes to next convo step
            _currentConvoStepIndex++;
        }

        // Loads the appropriate next step / branch;
        LoadConvoStep();
    }

    /// <summary>
    /// Call when updating the progress of the character's conversations
    /// </summary>
    /// <param name="characterTag"></param>
    /// <param name="convoIndex"></param>
    /// <param name="isBecomeKnown"></param>
    public void SetCharacterProgressTo(String characterTag, int convoIndex, bool isBecomeKnown = false){
        if(!_characters.ContainsKey(characterTag)){
            Debug.LogWarning($"[WARN]: Unknown Character Tag: {characterTag}");
            return;
        }

        CharacterConvoTracker tracker = _characters[characterTag];
        tracker.ConvoIndex = convoIndex;

        if(isBecomeKnown) MakeCharacterKnown(characterTag);
    }

    /// <summary>
    /// A function for updating the known status of a character
    /// </summary>
    /// <param name="updateValue"></param>
    public void MakeCharacterKnown(String characterTag){
        if(!_characters.ContainsKey(characterTag)){
            Debug.LogWarning($"[WARN]: Unknown Character Tag: {characterTag}");
            return;
        }

        CharacterConvoTracker tracker = _characters[characterTag];
        if(tracker.isCharacterKnown){
            Debug.LogWarning("[WARN]: Character was already known");
            return;
        }
        tracker.isCharacterKnown = true;
        Debug.Log("[TODO]: Insert code for character reveal here");
    }

    private void LoadConvoStep(){
        // Checks if we are at the branch's final convo step to load the options view
        if(_currentConvoStepIndex + 1 >= _currentConvo.ConvoSteps.Count){
            // Checks if the final option also has options, otherwise it defaults to non option view
            if(_currentConvo.EndingOptions.Count > 0){
                // Loads the option view
                _viewUpdater.SetChoicesView(
                    _characters[_currentCharacter].Character,
                    _currentConvo,        
                    _characters[_currentCharacter].isCharacterKnown 
                );
                return;
            }
        }
    
        // Loads the basic convo view
        _viewUpdater.SetConvoView(
            _characters[_currentCharacter].Character,
            GetCurrentConvoStep(),
            _characters[_currentCharacter].isCharacterKnown
        );
    }

    private ConvoBranchScriptable.ConvoStep GetCurrentConvoStep(){
        if(_currentConvoStepIndex <= -1 || 
            _currentConvo == null ||
            _currentConvoStepIndex >= _currentConvo.ConvoSteps.Count){
            Debug.LogWarning("[WARN]: Trying to access unknown convo step");
            return null;
        }

        return _currentConvo.ConvoSteps[_currentConvoStepIndex];
    }

    private void LoadCharacters(){
        _characters.Clear();
        foreach(CharacterConvoTracker tracker in _characterConvoProgress){
            _characters.Add(tracker.Character.CharacterTag, tracker);
        }
    }

    public static ConversationManager Instance {
        get {
            return _instance;
        }
    }
    private static ConversationManager _instance;
    void Awake()
    {
        if(Instance != null){
            Destroy(this);
            return;
        }

        _instance = this;        

        LoadCharacters();
    }
    void OnDestroy()
    {
        if(Instance == this)
            _instance = null;
    }
}
