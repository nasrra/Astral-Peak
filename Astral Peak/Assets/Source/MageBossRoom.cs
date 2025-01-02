using System.Collections.Generic;
using DocumentFormat.OpenXml.Office2019.Drawing.Model3D;
using Sounds;
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
        if(_x == 1){
            SceneLighting.instance.enable_light(id: "global",     enable: true);
            SceneLighting.instance.enable_light(id: "lightning",  enable: true);
            SceneLighting.instance.lerp_preset(_id: "global", _preset: _x, 2f);
            SceneLighting.instance.lerp_preset(_id: "lightning", _preset: _x, 2f);
        }
    }
    void link_events(){
        mage.phase_selected += handle_phase_switch;
    }
    void unlink_events(){
        mage.phase_selected -= handle_phase_switch;
    }
}
