using System;
using UnityEngine;

public class AudioPlayer : MonoBehaviour{
    SoundFunctions functions;
    public void set_functions(SoundFunctions _functions) => functions = _functions;
    public void play_sound(string sound_id) => functions.play_sound(sound_id);
    public void stop_sound(string sound_id) => functions.stop_sound(sound_id);
    public void play_ground_effected_sound(string sound_id) => functions.play_ground_effected_sound(sound_id);
    public void set_ground(string _ground) => functions.set_ground(_ground);
}
