using System.Collections.Generic;
using Sounds;
using UnityEditor.UnityLinker;
using UnityEngine;

public class MageBossRoom : BossRoomHandler{
    [SerializeField] SnowController snow_controller;
    [SerializeField] List<FogController> fog_controllers = new List<FogController>();
    List<SoundID> ambience = new List<SoundID>(){
        SoundID.SOFT_WIND,
        SoundID.HEAVY_WIND,
    };
    [SerializeField] TheMage mage;
    void Awake(){
        link_events();
    }
    void OnDestroy(){
        unlink_events();
    }
    protected override void check_world_state(){
        //throw new System.NotImplementedException();
    }
    void handle_phase_switch(int x){
        int _x = x-1;
        foreach(FogController fog in fog_controllers)
            fog.lerp_preset(_x);
        snow_controller.lerp_preset(_x);
        AudioManager.play_ambience(ambience[_x]);
    }
    void link_events(){
        mage.phase_selected += handle_phase_switch;
    }
    void unlink_events(){
        mage.phase_selected -= handle_phase_switch;
    }
}
