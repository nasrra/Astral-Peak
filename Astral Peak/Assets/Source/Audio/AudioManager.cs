using System.Collections.Generic;
using System.IO;
using Entropek;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AudioManager{
    static Dictionary<string,EventReference> loaded_references = new Dictionary<string, EventReference>();
    static Dictionary<string, sbyte> available_banks = new Dictionary<string, sbyte>();
    private static Bus 
        master_bus,
        music_bus,
        voice_bus,
        ambience_bus,
        sfx_bus;
    static EventInstance music_track;
    static EventInstance ambience_track;
    static string current_music_track = "";
    static string current_ambience_track = "";

    public static void initialize(){
        UnityEngine.Debug.Log(Settings.Instance.SourceBankPath+"/sfx");
        // load master banks.
        RuntimeManager.LoadBank("Master");
        RuntimeManager.LoadBank("Master.strings");
        // get buses
        master_bus      = RuntimeManager.GetBus("bus:/");
        music_bus       = RuntimeManager.GetBus("bus:/music");
        voice_bus       = RuntimeManager.GetBus("bus:/voice");
        sfx_bus         = RuntimeManager.GetBus("bus:/sfx");
        ambience_bus    = RuntimeManager.GetBus("bus:/ambience");
        load_volume_settings();
        set_available_banks();
        CustomSceneManager.loading_scene += unload_active_scene_bank;
        CustomSceneManager.loaded_scene += load_active_scene_bank;
        load_active_scene_bank();
    }

    public static void uninitialize(){
        CustomSceneManager.loading_scene -= unload_active_scene_bank;
        CustomSceneManager.loaded_scene -= load_active_scene_bank;
        save_volume_settings();
    }

    public static void set_available_banks(){
        if(Directory.Exists(Settings.Instance.SourceBankPath)){
            string[] bank_files = Directory.GetFiles(Settings.Instance.SourceBankPath, "*.bank");
            foreach(var file in bank_files){
                string file_name = Path.GetFileNameWithoutExtension(file);
                available_banks.Add(file_name, (sbyte)available_banks.Count);
                //UnityEngine.Debug.Log("found bank: "+file_name);
            }
        }
        else{
            throw new System.Exception("Bank Path does not exist: "+Settings.Instance.SourceBankPath);
        }
    }

    public static float get_master_volume(){
        master_bus.getVolume(out float _volume);
        return _volume;
    }
    public static float get_music_volume(){
        music_bus.getVolume(out float _volume);
        return _volume;
    }
    public static float get_voice_volume(){
        voice_bus.getVolume(out float _volume);
        return _volume;
    }
    public static float get_sfx_volume(){   
        sfx_bus.getVolume(out float _volume);
        return _volume;
    }
    public static float get_ambience_volume(){
        ambience_bus.getVolume(out float _volume);
        return _volume;
    }

    public static void set_master_volume(float _volume){
        master_bus.setVolume(_volume);
    } 
    public static void set_music_volume(float _volume){
        music_bus.setVolume(_volume);
    } 
    public static void set_voice_volume(float _volume){
        voice_bus.setVolume(_volume);
    } 
    public static void set_sfx_volume(float _volume){
        sfx_bus.setVolume(_volume);
    } 
    public static void set_ambience_volume(float _volume){
        ambience_bus.setVolume(_volume);
    }

    public static void save_volume_settings(){
        PlayerPrefs.SetString("master_volume", get_master_volume().ToString());
        PlayerPrefs.SetString("music_volume", get_music_volume().ToString());
        PlayerPrefs.SetString("voice_volume", get_voice_volume().ToString());
        PlayerPrefs.SetString("sfx_volume", get_sfx_volume().ToString());
        PlayerPrefs.SetString("ambience_volume", get_ambience_volume().ToString());
        PlayerPrefs.Save();
    }

    public static void load_volume_settings(){
        set_master_volume(float.Parse(PlayerPrefs.GetString("master_volume", get_master_volume().ToString())));
        set_music_volume(float.Parse(PlayerPrefs.GetString("music_volume", get_music_volume().ToString())));
        set_voice_volume(float.Parse(PlayerPrefs.GetString("voice_volume", get_voice_volume().ToString())));
        set_sfx_volume(float.Parse(PlayerPrefs.GetString("sfx_volume", get_sfx_volume().ToString())));
        set_ambience_volume(float.Parse(PlayerPrefs.GetString("ambience_volume", get_ambience_volume().ToString())));
    }

    public static void load_bank(string _bank_name){
        if(available_banks.ContainsKey(_bank_name) == false)
            return;
        if(RuntimeManager.HasBankLoaded(_bank_name)){
            UnityEngine.Debug.Log("Bank "+_bank_name+" has already been loaded!");
            return;
        }
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
        //UnityEngine.Debug.Log("Bank ["+_bank_name+"] loaded");
    }

    public static void unload_bank(string _bank_name){
        if(RuntimeManager.HasBankLoaded(_bank_name) == false){
            // UnityEngine.Debug.Log("Bank ["+_bank_name+"] has already been unloaded!");
            return;
        }
        Bank bank;
        RuntimeManager.StudioSystem.getBank("bank:/"+_bank_name, out bank);
        EventDescription[] descriptions;
        bank.getEventList(out descriptions);
        foreach(EventDescription description in descriptions){
            string path;
            description.getPath(out path);
            loaded_references.Remove(Path.GetFileNameWithoutExtension(path));
        }
        RuntimeManager.UnloadBank(_bank_name);
        //UnityEngine.Debug.Log("Bank ["+_bank_name+"] unloaded");
    }

    public static void load_bank(RoomType _room_type) => load_bank(room_type_banks[_room_type]);
    public static void unload_bank(RoomType _room_type) => unload_bank(room_type_banks[_room_type]);
    private static Dictionary<RoomType, string> room_type_banks = new Dictionary<RoomType, string>(){
        {RoomType.SHRINE, "room_shrine"},
        {RoomType.SNOW, "room_snow"},
    };


    // play one shot non-diegetic reference
    public static void play_non_diegetic_one_shot(string _event_name){
        RuntimeManager.PlayOneShot(loaded_references[_event_name]);
    }
    public static void play_diegetic_one_shot(string _event_name, GameObject _game_object) {
        RuntimeManager.PlayOneShotAttached(loaded_references[_event_name], _game_object);
    }
    public static EventReference get_event_reference(string _event_name){
        return loaded_references[_event_name];
    }


    public static float get_sound_length(string _event_name){
        RuntimeManager.GetEventDescription(loaded_references[_event_name]).getLength(out int time);
        return time;
    } 
    public static EventInstance create_event_instance(string _event_name) => RuntimeManager.CreateInstance(loaded_references[_event_name]);    

    private static void load_active_scene_bank(){
        load_bank(SceneManager.GetActiveScene().name);
    }

    private static void unload_active_scene_bank(){
        unload_bank(SceneManager.GetActiveScene().name);
    }

    public static void play_music_one_shot(string _event_name){
        if(current_music_track == _event_name)
            return;        
        stop_music();
        music_track = create_event_instance(_event_name);
        music_track.start();
        music_track.release();
        current_music_track = _event_name;
    }

    public static void play_music(string _event_name){
        if(current_ambience_track == _event_name)
            return;            
        stop_music();
        music_track = create_event_instance(_event_name);
        music_track.start();
        current_music_track = _event_name;
    }
    public static void stop_music(){
        music_track.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        music_track.release();
    }

    public static void play_ambience(string _event_name){
        if(current_ambience_track == _event_name)
            return;
        stop_ambience();
        ambience_track = create_event_instance(_event_name);
        ambience_track.start();
        current_ambience_track = _event_name;
    }
    public static void stop_ambience(){
        Log.MethodCall();
        ambience_track.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambience_track.release();
    }
}