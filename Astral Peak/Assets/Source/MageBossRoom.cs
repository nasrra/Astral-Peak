using System;
using System.Collections;
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
    List<Action> lighting_states = new List<Action>(){
        ()=>{// 0
            SceneLighting.instance.enable_light(id: "lightning",  enable: false);
            SceneLighting.instance.enable_light(id: "global",     enable: true);
            SceneLighting.instance.lerp_preset(_id: "global",    _preset: 0, 2f);
            SceneLighting.instance.lerp_preset(_id: "lightning", _preset: 0, 2f);},
        () =>{// 1
            SceneLighting.instance.enable_light(id: "lightning",  enable: true);
            SceneLighting.instance.enable_light(id: "global",     enable: true);
            SceneLighting.instance.lerp_preset(_id: "global",    _preset: 1, 2f);
            SceneLighting.instance.lerp_preset(_id: "lightning", _preset: 1, 2f);}
    };
    Dictionary<string, Cutscene> cutscenes = new Dictionary<string, Cutscene>(){
        {"opening",new MageOpening()},
        {"phase_1",new MagePhaseTransition()}
    };
    List<SoundID> ambience = new List<SoundID>(){
        SoundID.SOFT_WIND,
        SoundID.HEAVY_WIND,
    };
    void Awake(){
        instance = this;
        link_events();
    }
    void Start() => set_room_state(0);
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
    public void set_room_state(int x){
        AudioManager.play_ambience(ambience[x]);
        foreach(FogController fog in fog_controllers)
            fog.lerp_preset(x);
        snow_controller.lerp_preset(x);
        foreach(FogController fog in fog_controllers)
            fog.lerp_preset(x);
        lighting_states[x]();
        if(x == 1)
            phase_2_ambient_lightning.Play();
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


    void on_trigger_enter(Collider2D other){
        set_respawn_point();
        Player.instance.get_movement().halt();
        Player.instance.transform.position = cutscene_trigger.transform.position;
        cutscene_trigger.enabled = false;
        cutscene_trigger.trigger_enter -= on_trigger_enter;
        play_cutscene("phase_1");
    }

    void play_cutscene(string phase) => CutsceneManager.play(cutscenes[phase]);
    void fight_ended() => StartCoroutine(death_loop());
    IEnumerator death_loop(){
        yield return new WaitForSeconds(3);
        UiManager.instance.play_enemy_vanquished();
        yield return new WaitForSeconds(6);
        CustomSceneManager.load_scene("Shrine");
        CustomSceneManager.loaded_scene += play_altar_cutscene;
        Player.instance.enter_cutscene_state();
        yield break;
    }

    void play_altar_cutscene(){
        CutsceneManager.play(new ShrineAltarTwoCutscene());
        CustomSceneManager.loaded_scene -= play_altar_cutscene;
    }

    void link_events(){
        get_mage().phase_transition += play_cutscene;
        get_mage().death += fight_ended;
        cutscene_trigger.trigger_enter += on_trigger_enter;
    }
    void unlink_events(){
        get_mage().phase_transition += play_cutscene;
        get_mage().death -= fight_ended;
        cutscene_trigger.trigger_enter -= on_trigger_enter;
    }
}
