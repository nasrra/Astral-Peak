using System;
using System.Collections;
using System.Collections.Generic;
using Entropek;
using Sounds;
using UnityEngine;

public class MageBossRoom : BossRoomHandler{
    [SerializeField] Mage mage_script;
    [SerializeField] GameObject mage_object;
    [SerializeField] GameObject background_mage;
    [SerializeField] SnowController snow_controller;
    [SerializeField] List<FogController> fog_controllers = new List<FogController>();
    [SerializeField] LineParticleEmittersHandler line_particles;
    [SerializeField] SummoningCircleHandler summoning_circles;
    [SerializeField] ParticleHandler particles;
    [SerializeField] AudioSource phase_2_ambient_lightning;
    [SerializeField] MagicPlatformsController platforms;
    List<Action> lighting_states = new List<Action>(){
        ()=>{// 0
            SceneLighting.instance.enable_light(id: "global",     enable: true);
            SceneLighting.instance.enable_light(id: "additive",  enable: false);
            SceneLighting.instance.lerp_preset(_id: "global",    _preset: 0, 4f);
            SceneLighting.instance.lerp_preset(_id: "additive", _preset: 0, 4f);},
        () =>{// 1
            SceneLighting.instance.enable_light(id: "global",     enable: true);
            SceneLighting.instance.enable_light(id: "additive",  enable: true);
            SceneLighting.instance.lerp_preset(_id: "global",    _preset: 1, 4f);
            SceneLighting.instance.lerp_preset(_id: "additive", _preset: 1, 4f);}
    };
    List<SoundID> ambience = new List<SoundID>(){
        SoundID.SOFT_WIND,
        SoundID.HEAVY_WIND,
    };
    protected override void Awake(){
        link_events();
        cutscenes = new Dictionary<string, Func<Cutscene>>(){
            {"opening",()=>new Cutscenes.MageOpening()},
            {"phase_1",()=>new Cutscenes.MagePhaseTransition()}
        };
        base.Awake();
    }
    protected override void check_world_state(){
        if(GameManager.get_boss_state(1)==true){
            Log.MethodCall();
            unlink_fight_start_trigger();
            Player.instance.set_spawn_point(respawn_points[1].gameObject.name);
            exit.set_start_open(true);
        }
    }
    void Start() => set_room_state(0);
    void OnDestroy(){
        unlink_events();
    }
    public void enable_mage(bool x) => mage_object.SetActive(x);
    public void enable_background_mage(bool x) => background_mage.SetActive(x);
    public Mage get_mage() => mage_script;
    public MagicPlatformsController get_platforms()=>platforms;
    public BackgroundMage get_background_mage() => background_mage.GetComponent<BackgroundMage>();
    public void set_room_state(int x){
        AudioManager.play_ambience(ambience[x]);
        foreach(FogController fog in fog_controllers)
            fog.lerp_preset(x,4);
        snow_controller.lerp_preset(x);
        foreach(FogController fog in fog_controllers)
            fog.lerp_preset(x,4);
        lighting_states[x]();
        if(x == 1){
            phase_2_ambient_lightning.Play();
            phase_transition_lightning();
        }
    }
    public void emit_attraction_particles(){
        summoning_circles.turn_on(new(){2,3,4});
        for(int i = 1; i < 4; i++)
            particles.play_particle("stone_"+i);
    }
    public void stop_attraction_particles(){
        summoning_circles.turn_off(new(){2,3,4});
        for(int i = 1; i < 4; i++)
            particles.stop_particle("stone_"+i);
    }

    protected override void death_started(){
        platforms.stop_loop();
        platforms.destroy_platforms();
        base.death_started();
    }

    protected override int get_boss_id() => 1;
    protected override Cutscene get_altar_cutscene() => new ShrineAltarTwoCutscene();
    

    protected override void death_completed(){
        set_room_state(0);
        StopCoroutine("randomised_stone_lightning_loop");
        StartCoroutine(AudioClipHandler.fade_out(phase_2_ambient_lightning,.5f));
        base.death_completed();
    }

    public void phase_transition_lightning(){
        StartCoroutine(Util.timer(2,time_out:()=>line_particles.emit_once("stone_2_lightning")));
        StartCoroutine(Util.timer(3,time_out:()=>line_particles.emit_once("stone_4_lightning")));
        StartCoroutine(Util.timer(4,time_out:()=>line_particles.emit_once("staff_lightning_0")));
        StartCoroutine(Util.timer(4,time_out:()=>line_particles.emit_once("staff_lightning_1")));
        StartCoroutine(Util.timer(4,time_out:()=>line_particles.emit_once("staff_lightning_2")));
        StartCoroutine(Util.timer(5,time_out:()=>line_particles.emit_once("stone_2_lightning")));
        StartCoroutine(Util.timer(5,time_out:()=>line_particles.emit_once("stone_3_lightning")));
        StartCoroutine(Util.timer(5,time_out:()=>line_particles.emit_once("stone_4_lightning")));
    }

    public void start_randomised_stone_lightning() => StartCoroutine("randomised_stone_lightning_loop");
    IEnumerator randomised_stone_lightning_loop(){
        while(true){
            yield return new WaitForSeconds(8.5f);
            int x = UnityEngine.Random.Range(1,6);
            line_particles.emit_once($"stone_{x}_lightning");
            yield return null;
        }
    }


    void link_events(){
        link_mage();
        link_fight_start_trigger();
    }
    void unlink_events(){
        unlink_mage();
        unlink_fight_start_trigger();
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
