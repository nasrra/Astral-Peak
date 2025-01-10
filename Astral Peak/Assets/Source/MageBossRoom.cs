using System;
using System.Collections;
using System.Collections.Generic;
using Cutscenes;
using Deluz;
using Sounds;
using Unity.VisualScripting;
using UnityEngine;

public class MageBossRoom : BossRoomHandler{
    [SerializeField] Mage mage_script;
    [SerializeField] GameObject mage_object;
    [SerializeField] GameObject background_mage;
    [SerializeField] SnowController snow_controller;
    [SerializeField] List<FogController> fog_controllers = new List<FogController>();
    [SerializeField] SummoningCircleHandler summoning_circles;
    [SerializeField] ParticleHandler particles;
    [SerializeField] AudioSource phase_2_ambient_lightning;
    [SerializeField] MagicPlatformsController platforms;
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
        check_world_state();
        link_events();
    }
    void Start() => set_room_state(0);
    void OnDestroy(){
        unlink_events();
    }
    public void enable_mage(bool x) => mage_object.SetActive(x);
    public void enable_background_mage(bool x) => background_mage.SetActive(x);
    public Mage get_mage() => mage_script;
    public MagicPlatformsController get_platforms()=>platforms;
    public MageBackground get_background_mage() => background_mage.GetComponent<MageBackground>();
    protected override void check_world_state(){
        if(GameManager.get_boss_state(1)==true){
            cutscene_trigger.gameObject.SetActive(false);
            Player.instance.set_spawn_point(respawn_point.name);
            exit.opened();
        }
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
        play_cutscene("opening");
    }

    void play_cutscene(string phase) => CutsceneManager.play(cutscenes[phase]);
    void death_started(){
        platforms.stop_loop();
        platforms.destroy_platforms();
        EnemyManager.instance.destroy_all();
        ProjectileManager.instance.destroy_all();
        GameManager.set_boss_state(1,true);
    }
    void death_completed() => StartCoroutine(fight_ended());
    IEnumerator fight_ended(){
        UiManager.instance.play_enemy_vanquished();
        set_room_state(0);
        AudioClipHandler.fade_out(this,phase_2_ambient_lightning,.5f);
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
        link_mage();
        cutscene_trigger.trigger_enter += on_trigger_enter;
    }
    void unlink_events(){
        unlink_mage();
        cutscene_trigger.trigger_enter -= on_trigger_enter;
    }
    void link_mage(){
        mage_script.phase_transition    += play_cutscene;
        mage_script.death_started       += death_started;
        mage_script.death_completed     += death_completed;        
        mage_script.death_completed     += unlink_events;        
    }
    void unlink_mage(){
        if(mage_script==null)
            return;
        mage_script.phase_transition    -= play_cutscene;
        mage_script.death_started       -= death_started;
        mage_script.death_completed     -= death_completed;        
        mage_script.death_completed     -= unlink_events; 
    }
}
