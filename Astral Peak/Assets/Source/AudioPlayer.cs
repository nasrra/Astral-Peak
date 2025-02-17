using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioPlayer : MonoBehaviour{
    Dictionary<string, EventInstance> event_instances = new Dictionary<string, EventInstance>();

    void start_event_instance_loop(){
        StopAllCoroutines();
        StartCoroutine(update_event_instances());
    }
    void OnDestroy(){
        foreach(KeyValuePair<string, EventInstance> kvp in event_instances)
            stop_event_instance(kvp);
    }
    public void play_diegetic_one_shot(string _event_name){
        AudioManager.play_diegetic_one_shot(_event_name, gameObject);
    }
    public void play_non_diegetic_one_shot(string _event_name){
        AudioManager.play_non_diegetic_one_shot(_event_name);
    }
    public void play_diegetic_loop(string _event_name){
        EventInstance instance = AudioManager.create_event_instance(_event_name);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        if(event_instances.Count == 0){
            start_event_instance_loop();
        }
        event_instances.Add(_event_name, instance);
        instance.start();
    }
    public void stop_loop(string _event_name){
        EventInstance instance = event_instances[_event_name];
        stop_instance(instance);
        remove_instance(_event_name);
    }
    private void stop_event_instance(KeyValuePair<string, EventInstance> kvp){        
        stop_instance(kvp.Value);
        remove_instance(kvp.Key);
    }

    private void stop_instance(EventInstance _instance){
        _instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _instance.release();
    }

    private void remove_instance(string _event_name){
        event_instances.Remove(_event_name);
        if(event_instances.Count == 0){
            StopAllCoroutines();
        }
    }
    IEnumerator update_event_instances(){
        while(true){
            ATTRIBUTES_3D instance_attributes = RuntimeUtils.To3DAttributes(transform);
            yield return new WaitForFixedUpdate();
            foreach(EventInstance instance in event_instances.Values)
                instance.set3DAttributes(instance_attributes);
        }
    }
}
