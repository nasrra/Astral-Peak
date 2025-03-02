using System.Collections;
using System.Collections.Generic;
using Entropek;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioPlayer : MonoBehaviour{
    Dictionary<string, EventInstance> diegetic_instances = new Dictionary<string, EventInstance>();
    Dictionary<string, EventInstance> non_diegetic_instances = new Dictionary<string, EventInstance>();
    void OnEnable(){
        link();
    }
    void OnDisable(){
        StopAllCoroutines();
        stop_all_loops_immediate();
        unlink();
    }
    void start_diegetic_event_instance_loop(){
        StopAllCoroutines();
        StartCoroutine(update_diegetic_instances());
    }
    public void play_diegetic_one_shot_gameobject(string _event_name, GameObject _game_object){
        if(CutsceneManager.is_skipping() == true)
            return;
        AudioManager.play_diegetic_one_shot(_event_name, _game_object);
    }
    public void play_diegetic_one_shot(string _event_name){
        if(CutsceneManager.is_skipping() == true)
            return;
        AudioManager.play_diegetic_one_shot(_event_name, gameObject);
    }
    public void play_non_diegetic_one_shot(string _event_name){
        if(CutsceneManager.is_skipping() == true)
            return;
        AudioManager.play_non_diegetic_one_shot(_event_name);
    }
    public void play_diegetic_loop(string _event_name){
        if(CutsceneManager.is_skipping() == true)
            return;
        EventInstance instance = AudioManager.create_event_instance(_event_name);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        if(diegetic_instances.Count == 0){
            start_diegetic_event_instance_loop();
        }
        diegetic_instances.Add(_event_name, instance);
        instance.start();
    }
    public void play_non_diegetic_loop(string _event_name){
        if(CutsceneManager.is_skipping() == true)
            return;
        EventInstance instance = AudioManager.create_event_instance(_event_name);
        non_diegetic_instances.Add(_event_name, instance);
        instance.start();
    }
    public void stop_diegetic_loop(string _event_name){
        EventInstance instance = diegetic_instances[_event_name];
        stop_instance(instance);
        remove_diegetic_instance(_event_name);
    }

    public void stop_non_diegetic_loop(string _event_name){
        EventInstance instance = non_diegetic_instances[_event_name];
        stop_instance(instance);
        remove_non_diegetic_instance(_event_name);
    }
    public void play_non_diegetic_one_shot_instance(string _event_name){
        if(CutsceneManager.is_skipping() == true)
            return;
        AudioManager.play_non_diegetic_one_shot_instance(_event_name);
    }
    public void stop_all_loops(){
        // Log.MethodCall();
        foreach(KeyValuePair<string, EventInstance> kvp in diegetic_instances)
            stop_instance(kvp.Value);
        diegetic_instances.Clear();
        foreach(KeyValuePair<string, EventInstance> kvp in non_diegetic_instances)
            stop_instance(kvp.Value);
        non_diegetic_instances.Clear();
    }

    public void stop_all_loops_immediate(){
        // UnityEngine.Debug.Log(gameObject.name);
        foreach(KeyValuePair<string, EventInstance> kvp in diegetic_instances)
            stop_instance_immediate(kvp.Value);
        foreach(KeyValuePair<string, EventInstance> kvp in non_diegetic_instances)
            stop_instance_immediate(kvp.Value);
        diegetic_instances.Clear();
        non_diegetic_instances.Clear();
    }

    private void stop_instance(EventInstance _instance){
        _instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _instance.release();
    }

    private void stop_instance_immediate(EventInstance _instance){
        _instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _instance.release();
    }

    private void remove_diegetic_instance(string _event_name){
        diegetic_instances.Remove(_event_name);
        if(diegetic_instances.Count == 0){
            StopAllCoroutines();
        }
    }
    private void remove_non_diegetic_instance(string _event_name){
        non_diegetic_instances.Remove(_event_name);
    }
    IEnumerator update_diegetic_instances(){
        while(true){
            ATTRIBUTES_3D instance_attributes = RuntimeUtils.To3DAttributes(transform);
            yield return new WaitForFixedUpdate();
            foreach(EventInstance instance in diegetic_instances.Values)
                instance.set3DAttributes(instance_attributes);
        }
    }

    public void set_non_diegetic_instance_parameter(string _instance, string _paramter, float _value){
        non_diegetic_instances[_instance].setParameterByName(_paramter, _value);
    }

    public void set_diegetic_instance_parameter(string _instance, string _paramter, float _value){
        diegetic_instances[_instance].setParameterByName(_paramter, _value);
    }

    void destroy(){
        DestroyImmediate(this);
    }

    void link(){
        CustomSceneManager.unloading_scene += destroy;
    } 
    void unlink(){
        CustomSceneManager.unloading_scene -= destroy;
    }
}
