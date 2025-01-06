using UnityEngine;
using Sounds;
using System.Collections.Generic;
using System;

public abstract class SoundFunctions{
    protected string ground;
    protected Dictionary<string, Action> sound_functions;
    [SerializeField] protected Dictionary<string, AudioSource> looping_sources = new Dictionary<string, AudioSource>();
    protected MonoBehaviour audio_player;
    public void play_sound(string sound_id) => sound_functions[sound_id]();
    public void stop_sound(string sound_id){
        AudioClipHandler.fade_out(audio_player, looping_sources[sound_id], 1, destroy_source: true);
        looping_sources.Remove(sound_id);
    }
    public virtual void play_ground_effected_sound(string sound_id) => throw new Exception("This has not been implemented for this class!");
    protected abstract Dictionary<string, Action> create_sound_functions();
    protected SoundID random_id(List<SoundID> options) => options[UnityEngine.Random.Range(0, options.Count)];
    public SoundFunctions(MonoBehaviour _audio_player){
        audio_player = _audio_player;   
        sound_functions = create_sound_functions();
    }
    public void set_ground(string _ground) => ground = _ground;
    public SoundID choose_grounded(){
        switch(ground){
            case "Snow": return SoundID.SNOW_IMPACT_LIGHT;
            case "Stone": return SoundID.STONE_IMPACT_LIGHT;
        }
        return SoundID.NONE;
    }
    public void stop_all_loops(){
        if(looping_sources.Count > 0){
            List<string> keys = new List<string>(looping_sources.Keys);
            foreach(string key in keys)
                stop_sound(key);
        }
    }
}

public class CavalrySound : SoundFunctions{
    public CavalrySound(MonoBehaviour _audio_player) : base(_audio_player){}
    private List<SoundID> footsteps = new List<SoundID>(){
        SoundID.SNOW_FOOTSTEP_1,
        SoundID.SNOW_FOOTSTEP_2,
        SoundID.SNOW_FOOTSTEP_3,
        SoundID.SNOW_FOOTSTEP_4};
    protected override Dictionary<string, Action> create_sound_functions() =>
    new Dictionary<string, Action>(){
        {"arrow_knocked",()=>
            AudioClipHandler.play(
            SoundID.COIN_TOSS,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC)},
        {"bark",()=>
            AudioClipHandler.play(
            SoundID.DOG_BARK_1,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"bow_shot",()=>
            AudioClipHandler.play(
            SoundID.BOW_SHOT,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"front_strike_grab",()=>
            AudioClipHandler.play(
            SoundID.LEATHER_CONTORT_1,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC)},
        {"slam_hop",()=>
            AudioClipHandler.play(
            SoundID.MAGIC_1,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC)},
        {"slam_impact",()=>
            AudioClipHandler.play(
            SoundID.SNOW_IMPACT_HEAVY,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC)},
        {"howl",()=>
            AudioClipHandler.play(
            SoundID.WOLF_HOWL,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC)},
        {"melee",()=>
            AudioClipHandler.play(
            SoundID.MELEE_SWING_3,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC)},
        {"footstep",()=>
            AudioClipHandler.play(
            random_id(footsteps),
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"death_explosion",()=>
            AudioClipHandler.play(
            SoundID.MAGIC_EXPLOSION,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC)},
    };
}

public class RiderSound : SoundFunctions{
    public RiderSound(MonoBehaviour _audio_player) : base(_audio_player){}
    private List<SoundID> footsteps = new List<SoundID>(){
        SoundID.SNOW_FOOTSTEP_1,
        SoundID.SNOW_FOOTSTEP_2,
        SoundID.SNOW_FOOTSTEP_3,
        SoundID.SNOW_FOOTSTEP_4};
    protected override Dictionary<string, Action> create_sound_functions()
    => new Dictionary<string, Action>(){
        {"melee",()=>
            AudioClipHandler.play(
            SoundID.MELEE_SWING_3,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"footstep",()=>
            AudioClipHandler.play(
            random_id(footsteps),
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"whistle",()=>
            AudioClipHandler.play(
            SoundID.WHISTLE_LONG,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC)},
        {"arrow_shot",()=>
            AudioClipHandler.play(
            SoundID.BOW_SHOT,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"yell",()=>
            AudioClipHandler.play(
            SoundID.RIDER_YELL,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC)},
        {"bow_shot",()=>
            AudioClipHandler.play(
            SoundID.BOW_SHOT,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"magic_dash",()=>
            AudioClipHandler.play(
            SoundID.MAGIC_1,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"signature_slam",()=>
            AudioClipHandler.play(
            SoundID.SNOW_IMPACT_HEAVY,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
    };  
}

public class PlayerSound : SoundFunctions{
    private List<SoundID> snow_footsteps = new List<SoundID>(){
        SoundID.SNOW_FOOTSTEP_1,
        SoundID.SNOW_FOOTSTEP_2,
        SoundID.SNOW_FOOTSTEP_3,
        SoundID.SNOW_FOOTSTEP_4};
    private List<SoundID> stone_footsteps = new List<SoundID>(){
        SoundID.STONE_FOOTSTEP_1,
        SoundID.STONE_FOOTSTEP_2,
        SoundID.STONE_FOOTSTEP_3,
        SoundID.STONE_FOOTSTEP_4};
    public PlayerSound(MonoBehaviour _audio_player) : base(_audio_player){}
    public override void play_ground_effected_sound(string sound_id){
        if(ground == "" || ground == null)
            return;
        sound_functions[ground+"_"+sound_id]();
    }
    protected override Dictionary<string, Action> create_sound_functions()
    => new Dictionary<string, Action>(){
        {"melee", ()   => 
            AudioClipHandler.play(
            SoundID.MELEE_SWING_1,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC)},
        {"dash", ()   => 
            AudioClipHandler.play(
            SoundID.WHOOSH_1,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC_RANDOMISED)},    
        {"Snow_footstep", ()=>
            AudioClipHandler.play(
            random_id(snow_footsteps),
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"Stone_footstep", ()=>
            AudioClipHandler.play(
            random_id(stone_footsteps),
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"melee_hit", ()=> 
            AudioClipHandler.play(
            SoundID.MELEE_HIT,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"Snow_jump", ()=>
            AudioClipHandler.play(
            SoundID.SNOW_IMPACT_LIGHT,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"Stone_jump", ()=>
            AudioClipHandler.play(
            SoundID.STONE_IMPACT_LIGHT,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"damaged", ()=>
            AudioClipHandler.play(
            SoundID.DEEP_BOOM,
            audio_player: audio_player, 
            AudioSourceSettings.NON_DIEGETIC)},    
        {"death_explosion",()=>    
            AudioClipHandler.play(
            SoundID.MAGIC_EXPLOSION,
            audio_player: audio_player,
            AudioSourceSettings.DIEGETIC)},   
    };
}

public class HollowSound : SoundFunctions{
    public HollowSound(MonoBehaviour _audio_player) : base(_audio_player){}
    private List<SoundID> stone_footsteps = new List<SoundID>(){
        SoundID.STONE_FOOTSTEP_1,
        SoundID.STONE_FOOTSTEP_2,
        SoundID.STONE_FOOTSTEP_3,
        SoundID.STONE_FOOTSTEP_4};
    private List<SoundID> snow_footsteps = new List<SoundID>(){
        SoundID.SNOW_FOOTSTEP_1,
        SoundID.SNOW_FOOTSTEP_2,
        SoundID.SNOW_FOOTSTEP_3,
        SoundID.SNOW_FOOTSTEP_4};
    protected override Dictionary<string, Action> create_sound_functions()
    => new Dictionary<string, Action>(){
        {"yell", () =>
            AudioClipHandler.play(
            SoundID.RIDER_YELL,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC)},  
        {"Stone_footstep",()=>
            AudioClipHandler.play(
            random_id(stone_footsteps),
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC)},  
        {"Snow_footstep",()=>
            AudioClipHandler.play(
            random_id(snow_footsteps),
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC)},
        {"walk_rattle", () =>
            AudioClipHandler.play(
            SoundID.WOODEN_RATTLE_1,
            audio_player: audio_player,
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"idle_rattle",() =>
            AudioClipHandler.play(
            SoundID.WOODEN_RATTLE_2,
            audio_player: audio_player,
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"death_explosion", () =>
            AudioClipHandler.play(
            SoundID.MAGIC_EXPLOSION,
            audio_player: audio_player,
            AudioSourceSettings.DIEGETIC)},  
        {"death_rattle",() =>
        AudioClipHandler.play(
            SoundID.WOODEN_RATTLE_4,
            audio_player: audio_player,
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
    };
}

public class MageSound : SoundFunctions{
    public MageSound(MonoBehaviour _audio_player) : base(_audio_player){}
    private List<SoundID> snow_footsteps = new List<SoundID>(){
        SoundID.SNOW_FOOTSTEP_1,
        SoundID.SNOW_FOOTSTEP_2,
        SoundID.SNOW_FOOTSTEP_3,
        SoundID.SNOW_FOOTSTEP_4};
    private List<SoundID> thunder = new List<SoundID>(){
        SoundID.THUNDER_1,
        SoundID.THUNDER_2,
    };
    protected override Dictionary<string, Action> create_sound_functions()
    => new Dictionary<string, Action>(){
        {"yell", () =>
            AudioClipHandler.play(
            SoundID.RIDER_YELL,
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC)},    
        {"footstep",()=>
            AudioClipHandler.play(
            random_id(snow_footsteps),
            audio_player: audio_player, 
            AudioSourceSettings.DIEGETIC)},
        {"walk_rattle", () =>
            AudioClipHandler.play(
            SoundID.WOODEN_RATTLE_1,
            audio_player: audio_player,
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"idle_rattle",() =>
            AudioClipHandler.play(
            SoundID.WOODEN_RATTLE_2,
            audio_player: audio_player,
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"death_explosion", () =>
            AudioClipHandler.play(
            SoundID.MAGIC_EXPLOSION,
            audio_player: audio_player,
            AudioSourceSettings.DIEGETIC)},  
        {"death_rattle",() =>
            AudioClipHandler.play(
            SoundID.WOODEN_RATTLE_4,
            audio_player: audio_player,
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"staff_ambience",() =>
            looping_sources.Add("staff_ambience",
            AudioClipHandler.play(
            SoundID.BELL_CHIMES,
            audio_player: audio_player,
            AudioSourceSettings.DIEGETIC_RANDOMISED_LOOP))},
        {"small_magic_summon",() =>
            AudioClipHandler.play(
            SoundID.ELECTRICITY_2,
            audio_player: audio_player,
            AudioSourceSettings.DIEGETIC_RANDOMISED)},
        {"thunder", () =>
            AudioClipHandler.play(
            SoundID.THUNDER_2,
            audio_player: audio_player,
            AudioSourceSettings.NON_DIEGETIC_RANDOMISED)},
        {"electricity_loop", () =>
            looping_sources.Add("electricity_loop",
            AudioClipHandler.play(
            SoundID.ELECTRICITY_LOOP,
            audio_player: audio_player,
            AudioSourceSettings.NON_DIEGETIC_RANDOMISED_LOOP))},
        {"electric_burst",()=>
            AudioClipHandler.play(
            SoundID.ELECTRIC_BURST,
            audio_player: audio_player,
            AudioSourceSettings.NON_DIEGETIC_RANDOMISED)
        },
        {"ping",()=>
            AudioClipHandler.play(
            SoundID.PING,
            audio_player: audio_player,
            AudioSourceSettings.NON_DIEGETIC_RANDOMISED)
        },
    };
}