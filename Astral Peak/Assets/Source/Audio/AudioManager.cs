using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Entropek;
using FMOD.Studio;
using FMODUnity;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public static class AudioManager{
    static EventInstance music_track;
    static Dictionary<string,EventReference> loaded_references = new Dictionary<string, EventReference>();
    static Dictionary<string, sbyte> available_banks = new Dictionary<string, sbyte>();

    public static void initialize(){
        UnityEngine.Debug.Log(Settings.Instance.SourceBankPath+"/sfx");
        // load master banks.
        RuntimeManager.LoadBank("Master");
        RuntimeManager.LoadBank("Master.strings");
        set_available_banks();
    }

    public static void set_available_banks(){
        if(Directory.Exists(Settings.Instance.SourceBankPath)){
            string[] bank_files = Directory.GetFiles(Settings.Instance.SourceBankPath, "*.bank");
            foreach(var file in bank_files){
                string file_name = Path.GetFileNameWithoutExtension(file);
                available_banks.Add(file_name, (sbyte)available_banks.Count);
                UnityEngine.Debug.Log("found bank: "+file_name);
            }
        }
        else{
            throw new System.Exception("Bank Path does not exist: "+Settings.Instance.SourceBankPath);
        }
    }


    public static void load_bank(string _bank_name){
        if(available_banks.ContainsKey(_bank_name) == false)
            return;
        Log.MethodCall();
        RuntimeManager.LoadBank(_bank_name);
        Bank bank;
        FMOD.RESULT result = RuntimeManager.StudioSystem.getBank("bank:/"+_bank_name, out bank);
    
        if (result != FMOD.RESULT.OK){
            UnityEngine.Debug.LogError($"Failed to get bank {_bank_name}: {result}");
            return;
        }
        bank.getEventList(out EventDescription[] descriptions);
        foreach(EventDescription description in descriptions){
            description.getPath(out string path);
            EventReference reference = RuntimeManager.PathToEventReference(path);
            path = Path.GetFileNameWithoutExtension(path);
            loaded_references.Add(path,reference);
        }
        UnityEngine.Debug.Log("Bank ["+_bank_name+"] loaded");
    }

    public static void unload_bank(string _bank_name){
        if(available_banks.ContainsKey(_bank_name) == false)
            return;
        if(RuntimeManager.HasBankLoaded(_bank_name) == false){
            UnityEngine.Debug.Log("Bank ["+_bank_name+"] has already been unloaded!");
            return;
        }
        Bank bank;
        RuntimeManager.StudioSystem.getBank(_bank_name, out bank);
        EventDescription[] descriptions;
        bank.getEventList(out descriptions);
        foreach(EventDescription description in descriptions){
            string path;
            description.getPath(out path);
            loaded_references.Remove(Path.GetFileNameWithoutExtension(path));
        }
        RuntimeManager.UnloadBank(_bank_name);
        UnityEngine.Debug.Log("Bank ["+_bank_name+"] unloaded");
    }

    public static void load_bank(RoomType _room_type) => load_bank(room_type_banks[_room_type]);
    public static void unload_bank(RoomType _room_type) => unload_bank(room_type_banks[_room_type]);
    private static Dictionary<RoomType, string> room_type_banks = new Dictionary<RoomType, string>(){
        {RoomType.SHRINE, "room_shrine"},
        {RoomType.SNOW, "room_snow"},
    };

    public static void play_music(string _event_name){
        music_track = get_event_instance($"{FMODEventFolders.MUSIC}{_event_name}");
        music_track.start();

    }
    public static void stop_music(){
        music_track.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        music_track.release();
    }

    // play one shot non-diegetic reference
    public static void play_non_diegetic_one_shot(string _event_name){
        RuntimeManager.PlayOneShot(loaded_references[_event_name]);
    }
    public static void play_diegetic_one_shot(string _event_name, GameObject _game_object) {
        RuntimeManager.PlayOneShotAttached(loaded_references[_event_name], _game_object);
    }
    public static float get_sound_length(string _event_name){
        RuntimeManager.GetEventDescription(loaded_references[_event_name]).getLength(out int time);
        return time;
    } 
    public static EventReference get_event_reference(string event_name_path)    => RuntimeManager.PathToEventReference(event_name_path);    
    public static EventInstance get_event_instance(string event_name_path)      => RuntimeManager.CreateInstance(get_event_reference(event_name_path));    
}

public static class FMODEventFolders{
    public const string 
        SFX             = "event:/sfx/",
        MUSIC           = "event:/music/",
        VOICE           = "event:/voice/",
        SFX_SNOW        = SFX+"snow/",
        SFX_STONE       = SFX+"stone/",
        SFX_MELEE       = SFX+"melee/",
        SFX_MOVEMENT    = SFX+"movement/",
        VOICE_DOMINE    = VOICE+"domine/"
    ;
}

//Path.GetFileNameWithoutExtension(path);