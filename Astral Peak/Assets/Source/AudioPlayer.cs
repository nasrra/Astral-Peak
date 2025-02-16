using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioPlayer : MonoBehaviour{
    Dictionary<string, EventInstance> event_instances = new Dictionary<string, EventInstance>();

    public void play_diegetic_one_shot(string _event_name){
        AudioManager.play_diegetic_one_shot(_event_name, gameObject);
    }
    public void play_non_diegetic_one_shot(string _event_name){
        AudioManager.play_non_diegetic_one_shot(_event_name);
    }
}
