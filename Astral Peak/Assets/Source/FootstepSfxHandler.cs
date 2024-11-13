using UnityEngine;

public class FootstepSfxHandler : AudioClipHandler{
    Sound 
    snow_footstep_1,
    snow_footstep_2,
    snow_footstep_3,
    snow_footstep_4;
    AudioSource
        source;

    void Start(){
        fade_factor = 2;
        snow_footstep_1 = SoundLibrary.sfx["snow_footstep_1"]();
        snow_footstep_2 = SoundLibrary.sfx["snow_footstep_2"]();
        snow_footstep_3 = SoundLibrary.sfx["snow_footstep_3"]();
        snow_footstep_4 = SoundLibrary.sfx["snow_footstep_4"]();       
    }

    public void emit_sound() => play(choose_sound(),out source);
    Sound choose_sound(){
        int x = Random.Range(1,5);
        switch(x){
            case 1: return snow_footstep_1;
            case 2: return snow_footstep_2;
            case 3: return snow_footstep_3;
            case 4: return snow_footstep_4;
        }
        throw new System.Exception(x+" is not in range!");
    }
}
