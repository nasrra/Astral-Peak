using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneSounds{
    // scene sounds.
    public static readonly Dictionary<string, Func<List<SoundID>>> create = new Dictionary<string, Func<List<SoundID>>>(){
        {"WolfBossRoom", () => new List<SoundID>(){
            // MUSIC
            SoundID.WOLF_BOSS_MUSIC_1,
            SoundID.WOLF_BOSS_MUSIC_2,
            // SFX
            SoundID.BOW_SHOT,
            SoundID.COIN_TOSS,
            SoundID.DEEP_BOOM,
            SoundID.DOG_BARK_1,
            SoundID.LEATHER_CONTORT_1,
            SoundID.MAGIC_1,
            SoundID.MAGIC_EXPLOSION,
            SoundID.MELEE_HIT,
            SoundID.MELEE_SWING_1,
            SoundID.MELEE_SWING_2,
            SoundID.MELEE_SWING_3,
            SoundID.RIDER_YELL,
            SoundID.SNOW_FOOTSTEP_1,
            SoundID.SNOW_FOOTSTEP_2,
            SoundID.SNOW_FOOTSTEP_3,
            SoundID.SNOW_FOOTSTEP_4,
            SoundID.SNOW_IMPACT_HEAVY,
            SoundID.SNOW_IMPACT_LIGHT,
            SoundID.SNOW_JUMP,
            SoundID.SOFT_WIND,
            SoundID.STEAM,
            SoundID.WHOOSH_1,
            SoundID.WHISTLE_LONG,
            SoundID.WOLF_HOWL,
            SoundID.WOODEN_PING,        
        }},
    };
}
