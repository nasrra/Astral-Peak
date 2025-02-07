using System;
using UnityEngine;

public class AudioPlayer : MonoBehaviour{
    SoundFunctions functions;
    void Start() => functions?.set_entity(this);
    public void set_functions(SoundFunctions _functions) => functions = _functions;
    public void play_sound(string sound_id) => functions.play_sound(sound_id);
    public void stop_sound(string sound_id) => functions.stop_sound(sound_id);
    public void stop_all_loops()           => functions.stop_all_loops();
    public void play_ground_effected_sound(string sound_id) => functions.play_ground_effected_sound(sound_id);
    public void set_ground(string _ground) => functions.set_ground(_ground);
    public void set_audio_player(GameObject _audio_player) => functions.set_audio_player(_audio_player);
}
