using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class AudioPlayer : MonoBehaviour{
    Dictionary<string, EventInstance> event_instances = new Dictionary<string, EventInstance>();
    void OnDestroy(){
        foreach(EventInstance instance in event_instances.Values){
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.release();
        }
    }
    public void play_diegetic_one_shot(string _event_name){
        AudioManager.play_diegetic_one_shot(_event_name, gameObject);
    }
    public void play_non_diegetic_one_shot(string _event_name){
        AudioManager.play_non_diegetic_one_shot(_event_name);
    }
}
