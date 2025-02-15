using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioPlayer : MonoBehaviour{
    Dictionary<string, EventInstance> event_instances = new Dictionary<string, EventInstance>();
    Dictionary<string, EventReference> event_references = new Dictionary<string, EventReference>();
    
    public void initialize(AudioPlayerEventDataPackage data){
        initialize(data.get_event_references(), data.get_event_instances());
    }

    public void initialize(List<AudioPlayerEventData> _event_references, List<AudioPlayerEventData> _event_instances){
        foreach(AudioPlayerEventData data in _event_references)
            event_references.Add(data.event_name, AudioManager.get_event_reference($"{data.folder_path}{data.event_name}"));
        foreach(AudioPlayerEventData data in _event_instances)
            event_instances.Add(data.event_name, AudioManager.get_event_instance($"{data.folder_path}{data.event_name}"));
    }

    public void play_diegetic_one_shot(string _event_name){
        RuntimeManager.PlayOneShot(event_references[_event_name], transform.position);
    }
    public void play_non_diegetic_one_shot(string _event_name){
        RuntimeManager.PlayOneShot(event_references[_event_name]);
    }
    public void play_non_diegetic_event_instance(string _event_name){
        event_instances[_event_name].start();
    } 
    public void play_diegetic_event_instance(string _event_name){
        EventInstance instance = event_instances[_event_name];
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        instance.start();
    } 
    public void stop_event_instance(string _event_name){
        EventInstance instance = event_instances[_event_name];
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    } 
}

public struct AudioPlayerEventData{
    public readonly string
        folder_path,
        event_name;
    public AudioPlayerEventData(string _folder_path, string _event_name){
        folder_path = _folder_path;
        event_name = _event_name;
    }
}

public interface AudioPlayerEventDataPackage{
    public List<AudioPlayerEventData> get_event_references();
    public List<AudioPlayerEventData> get_event_instances();
}
