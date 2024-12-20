using UnityEngine.Audio;
using UnityEngine;

namespace Sounds{
public interface Sound{
    public AudioClip clip();
    public AudioMixerGroup group();
    public SoundID id();
    public float volume()    => 1;
    public float max_pitch() => 1;
    public float min_pitch() => 1;
    public float randomise_pitch() => UnityEngine.Random.Range(min_pitch(), max_pitch());
}

public struct None : Sound{
    public SoundID id()             => SoundID.NONE;
    public AudioClip clip()         => SoundLibrary.load_sfx("silence");
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
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 0.8f;
    public float volume()           => 1.0f;
}
//
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

public struct DomineAnOffering : Sound{
    public SoundID id()             => SoundID.DOMINE_AN_OFFERING;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_an_offering");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineBurnedWoman : Sound{
    public SoundID id()             => SoundID.DOMINE_BURNED_WOMAN;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_burned_woman");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineCursedOne : Sound{
    public SoundID id()             => SoundID.DOMINE_CURSED_ONE;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_cursed_one");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineDeadWoman : Sound{
    public SoundID id()             => SoundID.DOMINE_DEAD_WOMAN;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_dead_woman");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch() => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineEdgeOfWorld : Sound{
    public SoundID id()             => SoundID.DOMINE_EDGE_OF_WORLD;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_edge_of_world");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineEnterRooom : Sound{
    public SoundID id()             => SoundID.DOMINE_ENTER_ROOM;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_enter_room");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineFixWoman : Sound{
    public SoundID id()             => SoundID.DOMINE_FIX_WOMAN;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_fix_woman");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineFoolOrBrave : Sound{
    public SoundID id()             => SoundID.DOMINE_FOOL_OR_BRAVE;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_fool_or_brave");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineItsExpensive : Sound{
    public SoundID id()             => SoundID.DOMINE_ITS_EXPENSIVE;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_its_expensive");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineLostSoul : Sound{
    public SoundID id()             => SoundID.DOMINE_LOST_SOUL;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_lost_soul");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineNoHope : Sound{
    public SoundID id()             => SoundID.DOMINE_NO_HOPE;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_no_hope");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineOldDoor : Sound{
    public SoundID id()             => SoundID.DOMINE_OLD_DOOR;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_old_door");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineReviveWoman : Sound{
    public SoundID id()             => SoundID.DOMINE_REVIVE_WOMAN;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_revive_woman");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineSacrificeRitual : Sound{
    public SoundID id()             => SoundID.DOMINE_SACRIFICE_RITUAL;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_sacrifice_ritual");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineStrongWill : Sound{
    public SoundID id()             => SoundID.DOMINE_STRONG_WILL;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_strong_will");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineUnderArches : Sound{
    public SoundID id()             => SoundID.DOMINE_UNDER_ARCHES;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_under_arches");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineUnless : Sound{
    public SoundID id()             => SoundID.DOMINE_UNLESS;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_unless");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineWaitingMortal : Sound{
    public SoundID id()             => SoundID.DOMINE_WAITING_MORTAL;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_waiting_mortal");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineWalkAether : Sound{
    public SoundID id()             => SoundID.DOMINE_WALK_AETHER;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_walk_aether");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineWarning : Sound{
    public SoundID id()             => SoundID.DOMINE_WARNING;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_warning");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineNo : Sound{
    public SoundID id()             => SoundID.DOMINE_NO;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_no");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineYoureMortals : Sound{
    public SoundID id()             => SoundID.DOMINE_YOURE_MORTALS;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_youre_mortals");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineAshesToWind : Sound{
    public SoundID id()             => SoundID.DOMINE_ASHES_TO_WIND;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_ashes_to_wind");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineWhyHere : Sound{
    public SoundID id()             => SoundID.DOMINE_WHY_HERE;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_why_here");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct DomineMountainSummit : Sound{
    public SoundID id()             => SoundID.DOMINE_MOUNTAIN_SUMMIT;
    public AudioClip clip()         => SoundLibrary.load_sfx("domine_mountain_summit");
    public AudioMixerGroup group()  => AudioManager.voice_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => 1.0f;
    public float volume()           => 1.0f;
}

public struct Electricity1 : Sound{
    public SoundID id()             => SoundID.ELECTRICITY_1;
    public AudioClip clip()         => SoundLibrary.load_sfx("electricity_1");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => .85f;
    public float volume()           => 0.5f;
}

public struct Electricity2 : Sound{
    public SoundID id()             => SoundID.ELECTRICITY_2;
    public AudioClip clip()         => SoundLibrary.load_sfx("electricity_2");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => .85f;
    public float volume()           => 1f;
}

public struct Electricity3 : Sound{
    public SoundID id()             => SoundID.ELECTRICITY_3;
    public AudioClip clip()         => SoundLibrary.load_sfx("electricity_3");
    public AudioMixerGroup group()  => AudioManager.sfx_mixer;
    public float max_pitch()        => 1.0f;
    public float min_pitch()        => .85f;
    public float volume()           => 0.2f;
}
}