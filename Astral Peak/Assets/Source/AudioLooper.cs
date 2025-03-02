using UnityEngine;

public class AudioLooper : AudioPlayer{
    [SerializeField] string sound;
    [SerializeField] bool diegetic = true;
    protected override void OnEnable(){
        if(diegetic == true)
            play_diegetic_loop(sound);
        else
            play_non_diegetic_loop(sound);
        base.OnEnable();
    }
}
