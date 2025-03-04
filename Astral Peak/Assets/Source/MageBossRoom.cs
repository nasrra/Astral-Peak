using System;
using System.Collections;
using System.Collections.Generic;
using Entropek;
using FMODUnity;
using UnityEngine;

public class MageBossRoom : BossRoomHandler{
    [field: SerializeField] public Mage mage {get; private set;}
    [field: SerializeField] public BackgroundMage background_mage {get; private set;}
    [SerializeField] LineParticleEmittersHandler line_particles;
    [SerializeField] SummoningCircleHandler summoning_circles;
    [SerializeField] ParticleHandler particles;
    [SerializeField] AudioSpectrum audio_spectum;
    [SerializeField] MagicPlatformsController platforms;
    [SerializeField] GameObject button_prompt;
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
            unlink_fight_start_trigger();
            Player.instance.set_spawn_point(respawn_points[1].gameObject.name);
            exit.set_start_open(true);
        }
    }
    protected void OnDestroy(){
        unlink_events();
        if(room_state == 1)
            audio_spectum.uninitialize();
        // base.OnDestroy();
    }
    public MagicPlatformsController get_platforms()=>platforms;
    public override void set_room_state(int x){
        base.set_room_state(x);
        lighting_states[x]();
        if(x == 1){
            phase_transition_lightning();
            AudioManager.play_additive_ambience("ambience_thunder");
            audio_spectum.initialize(RuntimeManager.GetBus("bus:/ambience/additive"));
        }
    }

    public void enable_button_prompt() => button_prompt.SetActive(true);

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
        AudioManager.stop_additive_ambience();
        audio_spectum.uninitialize();
        StopCoroutine("randomised_stone_lightning_loop");
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
        mage.phase_transition    += play_cutscene;
        mage.death_started       += death_started;
        mage.death_completed     += death_completed;        
        mage.death_completed     += unlink_events;        
    }
    void unlink_mage(){
        mage.phase_transition    -= play_cutscene;
        mage.death_started       -= death_started;
        mage.death_completed     -= death_completed;        
        mage.death_completed     -= unlink_events; 
    }
}
