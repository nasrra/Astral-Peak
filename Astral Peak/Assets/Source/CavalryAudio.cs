using UnityEngine;

public class CavalryAudio : AudioClipHandler{
    Sound 
        arrow_knocked,
        bow_shot,
        front_strike_grab,
        ground_slam_hop,
        ground_slam_impact,
        sword_strike, 
        howl;
    Sound[] 
        bark = new Sound[3],
        snow_footstep = new Sound[4];
    AudioSource 
        source;

    void Start(){
        arrow_knocked       = SoundLibrary.sfx["coin_toss"]();
        bow_shot            = SoundLibrary.sfx["bow_shot"]();
        front_strike_grab   = SoundLibrary.sfx["leather_contort_1"]();
        bark[0]             = SoundLibrary.sfx["dog_bark_1"]();
        bark[1]             = SoundLibrary.sfx["dog_bark_2"]();
        bark[2]             = SoundLibrary.sfx["dog_bark_3"]();
        sword_strike        = SoundLibrary.sfx["melee_swing_2"]();
        howl                = SoundLibrary.sfx["wolf_howl"]();
        snow_footstep[0]    = SoundLibrary.sfx["snow_footstep_1"]();
        snow_footstep[1]    = SoundLibrary.sfx["snow_footstep_2"]();
        snow_footstep[2]    = SoundLibrary.sfx["snow_footstep_3"]();
        snow_footstep[3]    = SoundLibrary.sfx["snow_footstep_4"]();       
        ground_slam_hop     = SoundLibrary.sfx["magic_1"]();
        ground_slam_impact  = SoundLibrary.sfx["snow_impact_heavy"]();
    }

    public void emit_arrow_knocked()        => play(arrow_knocked, out source);
    public void emit_bow_shot()             => play(bow_shot, out source);
    public void emit_ground_slam_impact()   => play(ground_slam_impact, out source);
    public void emit_ground_slam_hop()      => play(ground_slam_hop,out source);
    public void emit_sword_strike()         => play(sword_strike,out source);
    public void emit_howl()                 => play(howl,out source);
    public void emit_footsteps()            => play(snow_footstep[Random.Range(0,4)],out source);
    public void emit_bark()                 => play(bark[Random.Range(0,3)], out source);
    public void emit_front_strike_grab()    => play(front_strike_grab, out source);
}
