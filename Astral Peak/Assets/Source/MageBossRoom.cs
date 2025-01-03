using System.Collections.Generic;
using Sounds;
using UnityEngine;

public class MageBossRoom : BossRoomHandler{
    [SerializeField] GameObject mage;
    [SerializeField] GameObject background_mage;
    [SerializeField] SnowController snow_controller;
    [SerializeField] List<FogController> fog_controllers = new List<FogController>();
    [SerializeField] AudioSource phase_2_ambient_lightning;
    List<SoundID> ambience = new List<SoundID>(){
        SoundID.SOFT_WIND,
        SoundID.HEAVY_WIND,
    };
    void Awake(){
        instance = this;
        link_events();
    }
    void OnDestroy(){
        unlink_events();
    }
    public void enable_mage(bool x) => mage.gameObject.SetActive(x);
    public void enable_background_mage(bool x) => background_mage.gameObject.SetActive(x);
    public TheMage get_mage() => mage.GetComponent<TheMage>();
    public MageBackground get_background_mage() => background_mage.GetComponent<MageBackground>();
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
            phase_2_ambient_lightning.Play();
        }
    }
    void link_events(){
        mage.GetComponent<TheMage>().phase_selected += handle_phase_switch;
    }
    void unlink_events(){
        mage.GetComponent<TheMage>().phase_selected -= handle_phase_switch;
    }
}
