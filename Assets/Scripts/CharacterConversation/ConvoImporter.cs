using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class ConvoImporter : MonoBehaviour
{
    [SerializeField] private string CONVO_FILEPATH = "/Convos/";
    [SerializeField] private string CHARACTER_FILEPATH = "/Characters/";
    [SerializeField] private string PORTRAITS_FILEPATH = "/Sprites/Portraits/";
    [SerializeField] private string ICONS_FILEPATH = "/Sprites/Icons/";
    [SerializeField] private Vector2 SPRITE_PIVOTS = new(0.5f, 0.5f);
    [SerializeField] private float SPRITE_PIXELS_PER_UNIT = 100.0f;
    [SerializeField] private bool _importConvo;
    [SerializeField] private bool _exportConvo;
    [SerializeField] private bool _importCharacter;
    [SerializeField] private bool _exportCharacter;
    [SerializeField] private string _convoToImport;
    [SerializeField] private string _characterToImport;
    [SerializeField] private ConvoBranchScriptable _workingConvo;
    [SerializeField] private CharacterScriptable _workingCharacter;

    [SerializeField] private List<CharacterScriptable> _debugLoadedChars = new();
    [SerializeField] private List<ConvoBranchScriptable> _debugLoadedConvos = new();
    private Dictionary<string, CharacterScriptable> _loadedCharacters = new();
    private Dictionary<string, ConvoBranchScriptable> _loadedConvos = new();
    private Dictionary<string, Sprite> _loadedPortraits = new();
    private Dictionary<string, Sprite> _loadedSourceIcons = new();

    public CharacterScriptable GetCharacter(string characterTag){
        if(!_loadedCharacters.ContainsKey(characterTag)){
            // Debug.Log("Trying to load character from get");
            CharacterScriptable character = ImportCharacter(Application.streamingAssetsPath + characterTag );
            if(character == null) return null;
        }

        return _loadedCharacters[characterTag];
    }

    public ConvoBranchScriptable GetConvo(string convoTag){
        if(!_loadedConvos.ContainsKey(convoTag)){
            ConvoBranchScriptable convo = ImportConvo(convoTag);
            if(convo == null) return null;
        }

        return _loadedConvos[convoTag];
    }

    public Sprite GetCharacterPortrait(string portraitFile){
        if(!_loadedPortraits.ContainsKey(portraitFile)){
            Sprite portrait = ImportCharacterPortrait(portraitFile);
            if(portrait == null) return null;
        }

        return _loadedPortraits[portraitFile];
    }

    public Sprite GetSourceIcon(string iconFile){
        if(!_loadedSourceIcons.ContainsKey(iconFile)){
            Sprite icon = ImportSourceIcon(iconFile);
            if(icon == null) return null;
        }

        return _loadedSourceIcons[iconFile];
    }

    public void ImportAllCharacters(){
        _loadedConvos.Clear();
        _loadedCharacters.Clear();

        DirectoryInfo characterDir = new(Application.streamingAssetsPath + CHARACTER_FILEPATH);
        FileInfo[] characterFiles = characterDir.GetFiles("*.txt");
        foreach (FileInfo file in characterFiles) ImportCharacter(file.FullName, true);
    }

    private ConvoBranchScriptable ImportConvo(string convoTag){
        if(convoTag.CompareTo("") == 0) return null;

        string path = ConstructPath(CONVO_FILEPATH, convoTag + ".txt");

        if(!File.Exists(path)){
            Debug.LogWarning($"[WARN]: Non-existent file {path}");
            return null;
        } 

        string[] importLines = File.ReadAllLines(path);
        string importString = string.Join("", importLines);

        ConvoBranchScriptable convo = JsonConvert.DeserializeObject<ConvoBranchScriptable>(importString);

        if(convo == null){
            Debug.LogWarning($"[WARN]: Failed to json convert the following file \"{importString}\"");
            return null;
        } 

        if(convoTag.CompareTo(convo.BranchTag) != 0){
            Debug.LogWarning($"[WARN]: Convo expected tag does not match actual tag for (Import: {convo.BranchTag}) {path}, fixing");
        
            convo.BranchTag = convoTag;
            _workingConvo = convo;
            ExportWorkingConvo();
        }

        if(_loadedConvos.ContainsKey(convo.BranchTag)){
            Debug.LogWarning($"[WARN]: Tried to import {convo.BranchTag} convo more than once");
            return null;
        }

        _loadedConvos.Add(convo.BranchTag, convo);
        _debugLoadedConvos.Add(convo);

        foreach(ConvoBranchScriptable.BranchEndOptionLink linkedConvo in convo.EndingOptions){
            ImportConvo(linkedConvo.NextBranchTag);
        }

        return convo;
    }

    private void ExportWorkingConvo(){
        if(_workingConvo.BranchTag.CompareTo("") == 0) return;

        string outputString = JsonConvert.SerializeObject(_workingConvo, Formatting.Indented);
        StreamWriter file = File.CreateText(
            ConstructPath(CONVO_FILEPATH, _workingConvo.BranchTag + ".txt")
        );
        file.WriteLine(outputString);
        file.Close();
    }

    private CharacterScriptable ImportCharacter(string characterTag, bool isPath = false){
        string path;
        if(isPath)
            path = characterTag;
        else
            path = ConstructPath(CHARACTER_FILEPATH, characterTag + ".txt");

        if(!File.Exists(path)){
            Debug.LogWarning($"[WARN]: Non-existent file {path}");
            return null;
        }

        string[] importLines = File.ReadAllLines(path);
        string importString = string.Join("", importLines);

        CharacterScriptable character = JsonConvert.DeserializeObject<CharacterScriptable>(importString);

        if(character == null){
            Debug.LogWarning($"[WARN]: Failed to json convert the following file \"{importString}\"");
            return null;
        }

        _loadedCharacters.Add(character.CharacterTag, character);
        _debugLoadedChars.Add(character);

        ImportSourceIcon(character.IconFile);

        foreach(string convoTag in character.ConvoStartBranchTags) 
            ImportConvo(convoTag);
        foreach(CharacterScriptable.PortraitEmotion portrait in character.CharacterPortaits) 
            ImportCharacterPortrait(portrait.PortraitFile);

        return character;
    }

    private void ExportWorkingCharacter(){
        if(_workingCharacter.CharacterTag.CompareTo("") == 0) return;

        string outputString = JsonConvert.SerializeObject(_workingCharacter, Formatting.Indented);
        StreamWriter file = File.CreateText(
            ConstructPath(CHARACTER_FILEPATH, _workingCharacter.CharacterTag + ".txt")
        );
        file.WriteLine(outputString);
        file.Close();
    }

    private Sprite ImportCharacterPortrait(string filename)
    {
        string path = ConstructPath(PORTRAITS_FILEPATH, filename);
        if(!File.Exists(path)){
            Debug.LogWarning($"[WARN]: Non-existent file {path}");
            return null;
        }

        byte[] imageData = File.ReadAllBytes(path);

        Texture2D texture = new(1,1,TextureFormat.ARGB32, false);
        texture.LoadImage(imageData);

        Sprite newSprite = Sprite.Create(
            texture, 
            new Rect(0.0f, 0.0f, texture.width, texture.height),
            SPRITE_PIVOTS,
            SPRITE_PIXELS_PER_UNIT
        );

        if(newSprite == null){
            Debug.LogWarning($"[WARN]: Failed to create portrait sprite");
            return null;
        }

        _loadedPortraits.Add(filename, newSprite);

        return newSprite;
    }

    private Sprite ImportSourceIcon(string filename)
    {
        string path = ConstructPath(ICONS_FILEPATH, filename);
        if(!File.Exists(path)) return null;

        byte[] imageData = File.ReadAllBytes(path);

        Texture2D texture = new(1,1,TextureFormat.ARGB32, false);
        texture.LoadImage(imageData);

        Sprite newSprite = Sprite.Create(
            texture, 
            new Rect(0.0f, 0.0f, texture.width, texture.height),
            SPRITE_PIVOTS,
            SPRITE_PIXELS_PER_UNIT
        );

        if(newSprite == null){
            Debug.LogWarning($"[WARN]: Failed to create icon sprite");
            return null;
        } 

        _loadedPortraits.Add(filename, newSprite);

        return newSprite;
    }

    private string ConstructPath(string generalFilepath, string fullFilename){
        return Application.streamingAssetsPath + generalFilepath + fullFilename;
    }

    public static ConvoImporter Instance { get; private set; }
    void Start()
    {
        if (Instance == null){
            Instance = this;
            ImportAllCharacters();
            JournalManager.Instance.Initialize();
            ConversationManager.Instance.Initialize();
        } 
        else Destroy(this);
        
    }
    void OnDestroy()
    {
        if(Instance == this) Instance = null;
    }

    void Update()
    {
        if(_exportCharacter){
            _exportCharacter = false;
            ExportWorkingCharacter();
        }
        if(_exportConvo){
            _exportConvo = false;
            ExportWorkingConvo();
        }
        if(_importCharacter){
            _importCharacter = false;
            if(_loadedCharacters.ContainsKey(_characterToImport))
                _workingCharacter = _loadedCharacters[_characterToImport];
            else
                _workingCharacter = ImportCharacter(_characterToImport);
        }
        if(_importConvo){
            _importConvo = false;
            if(_loadedConvos.ContainsKey(_convoToImport))
                _workingConvo = _loadedConvos[_convoToImport];
            else
                _workingConvo = ImportConvo(_convoToImport);
        }
    }
}
