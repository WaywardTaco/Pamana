using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class ConvoImporter : MonoBehaviour
{
    [SerializeField] private string CONVO_FILEPATH;
    [SerializeField] private string CHARACTER_FILEPATH;
    [SerializeField] private string PORTRAITS_FILEPATH;
    [SerializeField] private bool _importConvo;
    [SerializeField] private bool _exportConvo;
    [SerializeField] private bool _importCharacter;
    [SerializeField] private bool _exportCharacter;
    [SerializeField] private String _convoToImport;
    [SerializeField] private ConvoBranchScriptable _workingConvo;
    [SerializeField] private CharacterScriptable _workingCharacter;

    private Dictionary<String, CharacterScriptable> LoadedCharacters = new();
    private Dictionary<String, ConvoBranchScriptable> LoadedConvos = new();
    private Dictionary<String, Sprite> LoadedPortraits = new();

    public CharacterScriptable GetCharacter(String CharacterTag){
        if(!LoadedCharacters.ContainsKey(CharacterTag)){
            CharacterScriptable character = ImportCharacter(Application.streamingAssetsPath + CharacterTag );
            if(character == null) return null;
        }

        return LoadedCharacters[CharacterTag];
    }

    public ConvoBranchScriptable GetConvo(String ConvoTag){
        if(!LoadedConvos.ContainsKey(ConvoTag)){
            ConvoBranchScriptable convo = ImportConvo(ConvoTag);
            if(convo == null) return null;
        }

        return LoadedConvos[ConvoTag];
    }

    public Sprite GetCharacterPortrait(String PortraitFile){
        if(!LoadedPortraits.ContainsKey(PortraitFile)){
            Sprite portrait = ImportCharacterPortrait(PortraitFile);
            if(portrait == null) return null;
        }

        return LoadedPortraits[PortraitFile];
    }

    public void ImportAllCharacters(){
        LoadedConvos.Clear();
        LoadedCharacters.Clear();

        DirectoryInfo characterDir = new(Application.streamingAssetsPath + CHARACTER_FILEPATH);
        FileInfo[] characterFiles = characterDir.GetFiles("*.txt");
        foreach (FileInfo file in characterFiles) ImportCharacter(file.FullName);
    }

    private ConvoBranchScriptable ImportConvo(String path){
        return null;
    }

    private void ExportConvo(String path){

    }

    private CharacterScriptable ImportCharacter(String characterTag, bool isPath = false){
        string fullPath;
        if(isPath)
        fullPath = characterTag;
        else
            fullPath = Application.streamingAssetsPath + CHARACTER_FILEPATH + characterTag + ".txt";

        if(!File.Exists(fullPath)) return null;

        string[] importLines = File.ReadAllLines(fullPath);
        string importString = string.Join("", importLines);

        CharacterScriptable character = JsonConvert.DeserializeObject<CharacterScriptable>(importString);

        if(character == null) return null;

        LoadedCharacters.Add(character.CharacterTag, character);

        // foreach(ConvoBranchScriptable convo in character.ConvoStartBranchTags){
        //     ImportConvo();
        // }

        return character;
    }

    private void ExportCharacter(String path){

    }

    private Sprite ImportCharacterPortrait(String path){
        return null;
    }

    public static ConvoImporter Instance { get; private set; }
    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    void OnDestroy()
    {
        if(Instance == this) Instance = null;
    }
}
