using UnityEngine.Audio;
using UnityEngine;

namespace Sounds{
public interface Sound{
    public AudioClip clip();
    public AudioMixerGroup group();
    public SoundID id();
    public float volume();
    public float max_pitch();
    public float min_pitch();
    public float randomise_pitch() => UnityEngine.Random.Range(min_pitch(), max_pitch());
}

public struct None : Sound{
    public SoundID id()             => SoundID.NONE;
    public AudioClip clip()         => SoundLibrary.load_sfx("none");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 0f;
    public float min_pitch()        => 0f;
    public float volume()           => 0f;    
}

public struct WolfBossMusic1 : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_music("ABRN_run_part_1");
    public SoundID id()             => SoundID.WOLF_BOSS_MUSIC_1;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.music_mixer;
    public float max_pitch()        => 1;
    public float min_pitch()        => 1;
    public float volume()           => .8f;
}

public struct WolfBossMusic2 : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_music("ABRN_run_part_2");
    public SoundID id()             => SoundID.WOLF_BOSS_MUSIC_2;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.music_mixer;
    public float max_pitch()        => 1;
    public float min_pitch()        => 1;
    public float volume()           => .8f;
}

public struct DomineMusic : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_music("domine_theme");
    public SoundID id()             => SoundID.DOMINE_MUSIC;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.music_mixer;
    public float max_pitch()        => 1;
    public float min_pitch()        => 1;
    public float volume()           => .8f;
}

public struct HollowMusic : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_music("hollow_theme");
    public SoundID id()             => SoundID.HOLLOW_MUSIC;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.music_mixer;
    public float max_pitch()        => 1;
    public float min_pitch()        => 1;
    public float volume()           => .8f;
}

public struct AltarMusic : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_music("altar_theme");
    public SoundID id()             => SoundID.ALTAR_MUSIC;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.music_mixer;
    public float max_pitch()        => 1;
    public float min_pitch()        => 1;
    public float volume()           => .8f;
}

public struct SnowFootstep1 : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_sfx("snow_footstep_1");
    public SoundID id()             => SoundID.SNOW_FOOTSTEP_1;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct SnowFootstep2 : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_sfx("snow_footstep_2");
    public SoundID id()             => SoundID.SNOW_FOOTSTEP_2;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct SnowFootstep3 : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_sfx("snow_footstep_3");
    public SoundID id()             => SoundID.SNOW_FOOTSTEP_3;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct SnowJump : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_sfx("snow_jump");
    public SoundID id()             => SoundID.SNOW_JUMP;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct SnowFootstep4 : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_sfx("snow_footstep_4");
    public SoundID id()             => SoundID.SNOW_FOOTSTEP_4;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct SnowImpactHeavy : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_sfx("snow_impact_heavy");
    public SoundID id()             => SoundID.SNOW_IMPACT_HEAVY;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 0.6f;
}

public struct SnowImpactLight : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_sfx("snow_impact_light");
    public SoundID id()             => SoundID.SNOW_IMPACT_LIGHT;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct StoneFootstep1 : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_sfx("stone_footstep_1");
    public SoundID id()             => SoundID.STONE_FOOTSTEP_1;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct StoneFootstep2 : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_sfx("stone_footstep_2");
    public SoundID id()             => SoundID.STONE_FOOTSTEP_2;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct StoneFootstep3 : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_sfx("stone_footstep_3");
    public SoundID id()             => SoundID.STONE_FOOTSTEP_3;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct StoneFootstep4 : Sound{
    public SoundID id()             => SoundID.STONE_FOOTSTEP_4;
    public AudioClip clip()         => SoundLibrary.load_sfx("stone_footstep_4");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct StoneImpactLight : Sound{
    public SoundID id()             => SoundID.STONE_IMPACT_LIGHT;
    public AudioClip clip()         => SoundLibrary.load_sfx("stone_impact_light");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct SoftWind : Sound{
    public SoundID id()             => SoundID.SOFT_WIND;
    public AudioClip clip()         => SoundLibrary.load_sfx("soft_wind");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 0.8f;
}

public struct WolfHowl : Sound{
    public SoundID id()             => SoundID.WOLF_HOWL;
    public AudioClip clip()         => SoundLibrary.load_sfx("wolf_howl");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct MeleeHit : Sound{
    public SoundID id()             => SoundID.MELEE_HIT;
    public AudioClip clip()         => SoundLibrary.load_sfx("melee_hit");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct MeleeSwing1 : Sound{
    public SoundID id()             => SoundID.MELEE_SWING_1;
    public AudioClip clip()         => SoundLibrary.load_sfx("melee_swing_1");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct MeleeSwing2 : Sound{
    public SoundID id()             => SoundID.MELEE_SWING_2;
    public AudioClip clip()         => SoundLibrary.load_sfx("melee_swing_2");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct MeleeSwing3 : Sound{
    public SoundID id()             => SoundID.MELEE_SWING_3;
    public AudioClip clip()         => SoundLibrary.load_sfx("melee_swing_3");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct Magic1 : Sound{
    public SoundID id()             => SoundID.MAGIC_1;
    public AudioClip clip()         => SoundLibrary.load_sfx("magic_1");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct MagicExplosion : Sound{
    public SoundID id()             => SoundID.MAGIC_EXPLOSION;
    public AudioClip clip()         => SoundLibrary.load_sfx("magic_explosion");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct WhistleLong : Sound{
    public SoundID id()             => SoundID.WHISTLE_LONG;
    public AudioClip clip()         => SoundLibrary.load_sfx("whistle_long");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct Whoosh1 : Sound{
    public SoundID id()             => SoundID.WHOOSH_1;
    public AudioClip clip()         => SoundLibrary.load_sfx("whoosh_1");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct DogBark1 : Sound{
    public SoundID id()             => SoundID.DOG_BARK_1;
    public AudioClip clip()         => SoundLibrary.load_sfx("dog_bark_1");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct BowShot : Sound{
    public SoundID id()             => SoundID.BOW_SHOT;
    public AudioClip clip()         => SoundLibrary.load_sfx("bow_shot");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct CoinToss : Sound{
    public SoundID id()             => SoundID.COIN_TOSS;
    public AudioClip clip()         => SoundLibrary.load_sfx("coin_toss");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct RiderYell : Sound{
    public SoundID id()             => SoundID.RIDER_YELL;
    public AudioClip clip()         => SoundLibrary.load_sfx("rider_yell");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1f;
}

public struct WoodenPing : Sound{
    public SoundID id()             => SoundID.WOODEN_PING;
    public AudioClip clip()         => SoundLibrary.load_sfx("wooden_ping");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 0.7f;
}

public struct DeepBoom : Sound{
    public SoundID id()             => SoundID.DEEP_BOOM;
    public AudioClip clip()         => SoundLibrary.load_sfx("deep_boom");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 0.8f;
}

public struct Steam : Sound{
    public SoundID id()             => SoundID.STEAM;
    public AudioClip clip()         => SoundLibrary.load_sfx("steam");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 0.5f;
}

public struct SmallFire : Sound{
    public SoundID id()             => SoundID.SMALL_FIRE;
    public AudioClip clip()         => SoundLibrary.load_sfx("small_fire");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 0.75f;
}

public struct Daniel : Sound{
    AudioClip loaded_clip           => SoundLibrary.load_sfx("daniel");
    public SoundID id()             => SoundID.DANIEL;
    public AudioClip clip()         => loaded_clip;
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 0.8f;
    public float volume()           => 1.0f;
}

public struct WoodenRattle1 : Sound{
    public SoundID id()             => SoundID.WOODEN_RATTLE_1;
    public AudioClip clip()         => SoundLibrary.load_sfx("wooden_rattle_1");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1.0f;
}

public struct WoodenRattle2 : Sound{
    public SoundID id()             => SoundID.WOODEN_RATTLE_2;
    public AudioClip clip()         => SoundLibrary.load_sfx("wooden_rattle_2");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1.0f;
}

public struct WoodenRattle4 : Sound{
    public SoundID id()             => SoundID.WOODEN_RATTLE_4;
    public AudioClip clip()         => SoundLibrary.load_sfx("wooden_rattle_4");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 0.85f;
    public float volume()           => 1.0f;
}

public struct DeepThumping : Sound{
    public SoundID id()             => SoundID.DEEP_THUMPING;
    public AudioClip clip()         => SoundLibrary.load_sfx("deep_thumping");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}
}