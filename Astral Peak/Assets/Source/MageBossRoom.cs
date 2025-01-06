using System.Collections.Generic;
using Cutscenes;
using Sounds;
using UnityEngine;

public class MageBossRoom : BossRoomHandler{
    [SerializeField] GameObject mage;
    [SerializeField] GameObject background_mage;
    [SerializeField] SnowController snow_controller;
    [SerializeField] List<FogController> fog_controllers = new List<FogController>();
    [SerializeField] SummoningCircleHandler summoning_circles;
    [SerializeField] ParticleHandler particles;
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
    public void enable_mage(bool x) => mage.SetActive(x);
    public void enable_background_mage(bool x) => background_mage.SetActive(x);
    public Mage get_mage() => mage.GetComponent<Mage>();
    public MageBackground get_background_mage() => background_mage.GetComponent<MageBackground>();
    protected override void check_world_state(){
        //throw new System.NotImplementedException();
    }
    void handle_phase_switch(int x){
        reset_positions();
        int _x = x-1;
        if(x == 2){
            //mage.SetActive(false);
            CutsceneManager.play(new Cutscenes.MagePhaseTransition());
        }
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
    public void emit_attraction_particles(){
        summoning_circles.turn_on(new(){0,1,4});
        for(int i = 1; i < 4; i++)
            particles.play_particle("stone_"+i);
    }
    public void stop_attraction_particles(){
        summoning_circles.turn_off(new(){0,1,4});
        for(int i = 1; i < 4; i++)
            particles.stop_particle("stone_"+i);
    }
    protected void reset_positions(){
        Player.instance.transform.position = respawn_point.position;
        mage.transform.position = boss_start_point.position;
    }
    void on_trigger_enter(Collider2D other){
        set_respawn_point();
        Player.instance.get_movement().halt();
        Player.instance.transform.position = cutscene_trigger.transform.position;
        cutscene_trigger.enabled = false;
        CutsceneManager.play(new MageOpening());
        cutscene_trigger.trigger_enter -= on_trigger_enter;
    }
    void link_events(){
        mage.GetComponent<Mage>().phase_queued += handle_phase_switch;
        cutscene_trigger.trigger_enter += on_trigger_enter;
    }
    void unlink_events(){
        mage.GetComponent<Mage>().phase_queued -= handle_phase_switch;
        cutscene_trigger.trigger_enter -= on_trigger_enter;
    }
}
