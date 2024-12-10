using System.Collections.Generic;

public interface SoundSet{
    public List<SoundID> get_sound_ids();
}


public struct MeleeSoundSet : SoundSet{
    public List<SoundID> get_sound_ids() => sounds;
    List<SoundID> sounds => new List<SoundID>(){
        SoundID.MELEE_HIT,
        SoundID.MELEE_SWING_1,      
        SoundID.MELEE_SWING_2,
        SoundID.MELEE_SWING_3, 
    };
}

public struct SnowSoundSet : SoundSet{
    public List<SoundID> get_sound_ids() => sounds;
    List<SoundID> sounds => new List<SoundID>(){
        SoundID.SNOW_FOOTSTEP_1,
        SoundID.SNOW_FOOTSTEP_2,
        SoundID.SNOW_FOOTSTEP_3,
        SoundID.SNOW_FOOTSTEP_4,
        SoundID.SNOW_IMPACT_HEAVY,
        SoundID.SNOW_IMPACT_LIGHT,
    };
}

public struct MagicSoundSet : SoundSet{
    public List<SoundID> get_sound_ids() => sounds;
    List<SoundID> sounds => new List<SoundID>(){
        SoundID.MAGIC_1,
        SoundID.MAGIC_EXPLOSION,     
        SoundID.WHOOSH_1,       
    };
}

public struct UiSoundSet : SoundSet{
    public List<SoundID> get_sound_ids() => sounds;
    List<SoundID> sounds => new List<SoundID>(){
        SoundID.DEEP_BOOM,
        SoundID.WOODEN_PING,
    };
}

public struct WolfSoundSet : SoundSet{
    public List<SoundID> get_sound_ids() => sounds;
    List<SoundID> sounds => new List<SoundID>(){
        SoundID.DOG_BARK_1,  
        SoundID.WOLF_HOWL,
    };
}

public struct OutdoorAmbienceSoundSet : SoundSet{
    public List<SoundID> get_sound_ids() => sounds;
    List<SoundID> sounds => new List<SoundID>(){
        SoundID.SOFT_WIND,
    };
}

public struct AltarCutsceneSoundSet : SoundSet{
    public List<SoundID> get_sound_ids() => sounds;
    List<SoundID> sounds => new List<SoundID>(){
        SoundID.ALTAR_THEME,
        SoundID.DEEP_THUMPING
    };
}

public struct CavalrySoundSet : SoundSet{
    public List<SoundID> get_sound_ids() => sounds;
    List<SoundID> sounds => new List<SoundID>(){
        SoundID.WOLF_BOSS_MUSIC_1,
        SoundID.WOLF_BOSS_MUSIC_2,
    };
}


public struct FireSoundSet : SoundSet{
    public List<SoundID> get_sound_ids() => sounds;
    List<SoundID> sounds => new List<SoundID>(){
        SoundID.STEAM,
        SoundID.SMALL_FIRE,
    };
}


public struct DomineSoundSet : SoundSet{
    public List<SoundID> get_sound_ids() => sounds;
    List<SoundID> sounds => new List<SoundID>(){
        SoundID.DOMINE_VOICE_1,
        SoundID.DOMINE_VOICE_2,
        SoundID.DOMINE_VOICE_3,
        SoundID.DOMINE_VOICE_4,
        SoundID.DANIEL,
        SoundID.DOMINE_THEME,
    };
}

public struct RangedSoundSet : SoundSet{
    public List<SoundID> get_sound_ids() => sounds;
    List<SoundID> sounds => new List<SoundID>(){
        SoundID.BOW_SHOT,
        SoundID.COIN_TOSS,         
    };
}

public struct StoneSoundSet : SoundSet{
    public List<SoundID> get_sound_ids() => sounds;
    List<SoundID> sounds => new List<SoundID>(){
        SoundID.STONE_FOOTSTEP_1,
        SoundID.STONE_FOOTSTEP_2,
        SoundID.STONE_FOOTSTEP_3,
        SoundID.STONE_FOOTSTEP_4,
    };
}

public struct RiderSoundSet : SoundSet{
    public List<SoundID> get_sound_ids() => sounds;
    List<SoundID> sounds => new List<SoundID>(){
        SoundID.RIDER_YELL,
        SoundID.WHISTLE_LONG,
    };
}

public struct HollowSoundSet : SoundSet{
    public List<SoundID> get_sound_ids() => sounds;
    List<SoundID> sounds => new List<SoundID>(){
        SoundID.RIDER_YELL,
        SoundID.WOODEN_RATTLE_1,
        SoundID.WOODEN_RATTLE_2,
        SoundID.WOODEN_RATTLE_4,
        SoundID.HOLLOW_THEME,
    };
}