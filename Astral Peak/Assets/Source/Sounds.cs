using UnityEngine.Audio;
using UnityEngine;
using DocumentFormat.OpenXml.Office2019.Excel.RichData2;

namespace Sounds{
public class Sound{
    public readonly AudioClip clip;
    public readonly AudioMixerGroup group;
    public readonly SoundID id;
    public readonly float volume, max_pitch, min_pitch;
    public Sound(SoundID _id, string _clip, AudioMixerGroup _group, float _volume, float _max_pitch, float _min_pitch){
        id = _id;
        group = _group;
        clip = SoundLibrary.load(_group, _clip);
        volume = _volume;
        max_pitch = _max_pitch;
        min_pitch = _min_pitch;
    }
    public float randomise_pitch() => Random.Range(min_pitch, max_pitch);
}

public class None : Sound{
    public None() : base(
        _id:        SoundID.NONE,
        _clip:      "silence",
        _group:     AudioManager.sfx_mixer,
        _volume:    0f,
        _max_pitch: 0f,
        _min_pitch: 0f
    ){}    
}

public class WolfBossMusic1 : Sound{
    public WolfBossMusic1() : base(
        _id:        SoundID.WOLF_BOSS_MUSIC_1,
        _clip:      "wolf_boss_1",
        _group:     AudioManager.music_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}    
}

public class WolfBossMusic2 : Sound{
    public WolfBossMusic2() : base(
        _id:        SoundID.WOLF_BOSS_MUSIC_2,
        _clip:      "wolf_boss_2",
        _group:     AudioManager.music_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}    
}

public class MageBossMusic1 : Sound{
    public MageBossMusic1() : base(
        _id:        SoundID.MAGE_BOSS_MUSIC_1,
        _clip:      "mage_boss_1",
        _group:     AudioManager.music_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}    
}

public class MageBossMusic2 : Sound{
    public MageBossMusic2() : base(
        _id:        SoundID.MAGE_BOSS_MUSIC_2,
        _clip:      "mage_boss_2",
        _group:     AudioManager.music_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}    
}

public class AltarMusic : Sound{
    public AltarMusic() : base(
        _id:        SoundID.ALTAR_MUSIC,
        _clip:      "altar_theme",
        _group:     AudioManager.music_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}    
}

public class Introduction : Sound{
    public Introduction() : base(
        _id:        SoundID.INTRODUCTION_MUSIC,
        _clip:      "introduction",
        _group:     AudioManager.music_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}    
}

public class DomineMusic : Sound{
    public DomineMusic() : base(
        _id:        SoundID.DOMINE_MUSIC,
        _clip:      "domine_1",
        _group:     AudioManager.music_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}    
}

public class SnowFootstep1 : Sound {
    public SnowFootstep1() : base(
        _id:        SoundID.SNOW_FOOTSTEP_1,
        _clip:      "snow_footstep_1",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class SnowFootstep2 : Sound {
    public SnowFootstep2() : base(
        _id:        SoundID.SNOW_FOOTSTEP_2,
        _clip:      "snow_footstep_2",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class SnowFootstep3 : Sound {
    public SnowFootstep3() : base(
        _id:        SoundID.SNOW_FOOTSTEP_3,
        _clip:      "snow_footstep_3",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class SnowJump : Sound {
    public SnowJump() : base(
        _id:        SoundID.SNOW_JUMP,
        _clip:      "snow_jump",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}

public class SnowFootstep4 : Sound {
    public SnowFootstep4() : base(
        _id:        SoundID.SNOW_FOOTSTEP_4,
        _clip:      "snow_footstep_4",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class SnowImpactHeavy : Sound {
    public SnowImpactHeavy() : base(
        _id:        SoundID.SNOW_IMPACT_HEAVY,
        _clip:      "snow_impact_heavy",
        _group:     AudioManager.sfx_mixer,
        _volume:    0.6f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class SnowImpactLight : Sound {
    public SnowImpactLight() : base(
        _id:        SoundID.SNOW_IMPACT_LIGHT,
        _clip:      "snow_impact_light",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class StoneFootstep1 : Sound {
    public StoneFootstep1() : base(
        _id:        SoundID.STONE_FOOTSTEP_1,
        _clip:      "stone_footstep_1",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class StoneFootstep2 : Sound {
    public StoneFootstep2() : base(
        _id:        SoundID.STONE_FOOTSTEP_2,
        _clip:      "stone_footstep_2",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class StoneFootstep3 : Sound {
    public StoneFootstep3() : base(
        _id:        SoundID.STONE_FOOTSTEP_3,
        _clip:      "stone_footstep_3",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class StoneFootstep4 : Sound {
    public StoneFootstep4() : base(
        _id:        SoundID.STONE_FOOTSTEP_4,
        _clip:      "stone_footstep_4",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class StoneImpactLight : Sound {
    public StoneImpactLight() : base(
        _id:        SoundID.STONE_IMPACT_LIGHT,
        _clip:      "stone_impact_light",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class SoftWind : Sound {
    public SoftWind() : base(
        _id:        SoundID.SOFT_WIND,
        _clip:      "soft_wind",
        _group:     AudioManager.sfx_mixer,
        _volume:    0.8f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class WolfHowl : Sound {
    public WolfHowl() : base(
        _id:        SoundID.WOLF_HOWL,
        _clip:      "wolf_howl",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class MeleeHit : Sound {
    public MeleeHit() : base(
        _id:        SoundID.MELEE_HIT,
        _clip:      "melee_hit",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class MeleeSwing1 : Sound {
    public MeleeSwing1() : base(
        _id:        SoundID.MELEE_SWING_1,
        _clip:      "melee_swing_1",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class MeleeSwing2 : Sound {
    public MeleeSwing2() : base(
        _id:        SoundID.MELEE_SWING_2,
        _clip:      "melee_swing_2",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}
public class MeleeSwing3 : Sound {
    public MeleeSwing3() : base(
        _id:        SoundID.MELEE_SWING_3,
        _clip:      "melee_swing_3",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}


public class Magic1 : Sound {
    public Magic1() : base(
        _id:        SoundID.MAGIC_1,
        _clip:      "magic_1",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}

public class MagicExplosion : Sound {
    public MagicExplosion() : base(
        _id:        SoundID.MAGIC_EXPLOSION,
        _clip:      "magic_explosion",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}

public class WhistleLong : Sound {
    public WhistleLong() : base(
        _id:        SoundID.WHISTLE_LONG,
        _clip:      "whistle_long",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}

public class Whoosh1 : Sound {
    public Whoosh1() : base(
        _id:        SoundID.WHOOSH_1,
        _clip:      "whoosh_1",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}

public class DogBark1 : Sound {
    public DogBark1() : base(
        _id:        SoundID.DOG_BARK_1,
        _clip:      "dog_bark_1",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}

public class BowShot : Sound {
    public BowShot() : base(
        _id:        SoundID.BOW_SHOT,
        _clip:      "bow_shot",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}

public class CoinToss : Sound {
    public CoinToss() : base(
        _id:        SoundID.COIN_TOSS,
        _clip:      "coin_toss",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}


public class RiderYell : Sound {
    public RiderYell() : base(
        _id:        SoundID.RIDER_YELL,
        _clip:      "rider_yell",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}

public class WoodenPing : Sound {
    public WoodenPing() : base(
        _id:        SoundID.WOODEN_PING,
        _clip:      "wooden_ping",
        _group:     AudioManager.sfx_mixer,
        _volume:    0.7f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}

public class DeepBoom : Sound {
    public DeepBoom() : base(
        _id:        SoundID.DEEP_BOOM,
        _clip:      "deep_boom",
        _group:     AudioManager.sfx_mixer,
        _volume:    0.8f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}

public class Steam : Sound {
    public Steam() : base(
        _id:        SoundID.STEAM,
        _clip:      "steam",
        _group:     AudioManager.sfx_mixer,
        _volume:    0.5f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}

public class SmallFire : Sound {
    public SmallFire() : base(
        _id:        SoundID.SMALL_FIRE,
        _clip:      "small_fire",
        _group:     AudioManager.sfx_mixer,
        _volume:    0.75f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}

public class WoodenRattle1 : Sound {
    public WoodenRattle1() : base(
        _id:        SoundID.WOODEN_RATTLE_1,
        _clip:      "wooden_rattle_1",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ) {}
}


public class WoodenRattle2 : Sound {
    public WoodenRattle2() : base(
        _id:        SoundID.WOODEN_RATTLE_2,
        _clip:      "wooden_rattle_2",
        _group:     AudioManager.sfx_mixer,
        _volume:    1.0f,
        _max_pitch: 1.0f,
        _min_pitch: 0.85f
    ) {}
}

public class WoodenRattle4 : Sound {
    public WoodenRattle4() : base(
        _id:        SoundID.WOODEN_RATTLE_4,
        _clip:      "wooden_rattle_4",
        _group:     AudioManager.sfx_mixer,
        _volume:    1.0f,
        _max_pitch: 1.0f,
        _min_pitch: 0.85f
    ) {}
}

public class DeepThumping : Sound {
    public DeepThumping() : base(
        _id:        SoundID.DEEP_THUMPING,
        _clip:      "deep_thumping",
        _group:     AudioManager.sfx_mixer,
        _volume:    1.0f,
        _max_pitch: 1.0f,
        _min_pitch: 1.0f
    ) {}
}

public class DomineAnOffering : Sound {
    public DomineAnOffering() : base(
        _id:        SoundID.DOMINE_AN_OFFERING,
        _clip:      "domine_an_offering",
        _group:     AudioManager.voice_mixer,
        _volume:    1.0f,
        _max_pitch: 1.0f,
        _min_pitch: 1.0f
    ) {}
}

public class DomineBurnedWoman : Sound {
    public DomineBurnedWoman() : base(
        _id:        SoundID.DOMINE_BURNED_WOMAN,
        _clip:      "domine_burned_woman",
        _group:     AudioManager.voice_mixer,
        _volume:    1.0f,
        _max_pitch: 1.0f,
        _min_pitch: 1.0f
    ) {}
}

public class DomineCursedOne : Sound {
    public DomineCursedOne() : base(
        _id:        SoundID.DOMINE_CURSED_ONE,
        _clip:      "domine_cursed_one",
        _group:     AudioManager.voice_mixer,
        _volume:    1.0f,
        _max_pitch: 1.0f,
        _min_pitch: 1.0f
    ) {}
}

public class DomineDeadWoman : Sound {
    public DomineDeadWoman() : base(
        _id:        SoundID.DOMINE_DEAD_WOMAN,
        _clip:      "domine_dead_woman",
        _group:     AudioManager.voice_mixer,
        _volume:    1.0f,
        _max_pitch: 1.0f,
        _min_pitch: 1.0f
    ) {}
}

public class DomineEdgeOfWorld : Sound {
    public DomineEdgeOfWorld() : base(
        _id:        SoundID.DOMINE_EDGE_OF_WORLD,
        _clip:      "domine_edge_of_world",
        _group:     AudioManager.voice_mixer,
        _volume:    1.0f,
        _max_pitch: 1.0f,
        _min_pitch: 1.0f
    ) {}
}

public class DomineEnterRoom : Sound {
    public DomineEnterRoom() : base(
        _id:        SoundID.DOMINE_ENTER_ROOM,
        _clip:      "domine_enter_room",
        _group:     AudioManager.voice_mixer,
        _volume:    1.0f,
        _max_pitch: 1.0f,
        _min_pitch: 1.0f
    ) {}
}

public class DomineFixWoman : Sound {
    public DomineFixWoman() : base(
        _id:        SoundID.DOMINE_FIX_WOMAN,
        _clip:      "domine_fix_woman",
        _group:     AudioManager.voice_mixer,
        _volume:    1.0f,
        _max_pitch: 1.0f,
        _min_pitch: 1.0f
    ) {}
}

public class DomineFoolOrBrave : Sound {
    public DomineFoolOrBrave() : base(
        _id:        SoundID.DOMINE_FOOL_OR_BRAVE,
        _clip:      "domine_fool_or_brave",
        _group:     AudioManager.voice_mixer,
        _volume:    1.0f,
        _max_pitch: 1.0f,
        _min_pitch: 1.0f
    ) {}
}


public class DomineItsExpensive : Sound
{
    public DomineItsExpensive() : base(
        _id:        SoundID.DOMINE_ITS_EXPENSIVE,
        _clip:      "domine_its_expensive",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class DomineLostSoul : Sound
{
    public DomineLostSoul() : base(
        _id:        SoundID.DOMINE_LOST_SOUL,
        _clip:      "domine_lost_soul",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class DomineNoHope : Sound
{
    public DomineNoHope() : base(
        _id:        SoundID.DOMINE_NO_HOPE,
        _clip:      "domine_no_hope",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class DomineOldDoor : Sound
{
    public DomineOldDoor() : base(
        _id:        SoundID.DOMINE_OLD_DOOR,
        _clip:      "domine_old_door",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class DomineReviveWoman : Sound
{
    public DomineReviveWoman() : base(
        _id:        SoundID.DOMINE_REVIVE_WOMAN,
        _clip:      "domine_revive_woman",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class DomineSacrificeRitual : Sound
{
    public DomineSacrificeRitual() : base(
        _id:        SoundID.DOMINE_SACRIFICE_RITUAL,
        _clip:      "domine_sacrifice_ritual",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class DomineStrongWill : Sound
{
    public DomineStrongWill() : base(
        _id:        SoundID.DOMINE_STRONG_WILL,
        _clip:      "domine_strong_will",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class DomineUnderArches : Sound
{
    public DomineUnderArches() : base(
        _id:        SoundID.DOMINE_UNDER_ARCHES,
        _clip:      "domine_under_arches",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class DomineUnless : Sound
{
    public DomineUnless() : base(
        _id:        SoundID.DOMINE_UNLESS,
        _clip:      "domine_unless",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class DomineWaitingMortal : Sound
{
    public DomineWaitingMortal() : base(
        _id:        SoundID.DOMINE_WAITING_MORTAL,
        _clip:      "domine_waiting_mortal",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}


public class DomineWalkAether : Sound
{
    public DomineWalkAether() : base(
        _id:        SoundID.DOMINE_WALK_AETHER,
        _clip:      "domine_walk_aether",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class DomineWarning : Sound
{
    public DomineWarning() : base(
        _id:        SoundID.DOMINE_WARNING,
        _clip:      "domine_warning",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class DomineNo : Sound
{
    public DomineNo() : base(
        _id:        SoundID.DOMINE_NO,
        _clip:      "domine_no",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class DomineYoureMortals : Sound
{
    public DomineYoureMortals() : base(
        _id:        SoundID.DOMINE_YOURE_MORTALS,
        _clip:      "domine_youre_mortals",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class DomineAshesToWind : Sound
{
    public DomineAshesToWind() : base(
        _id:        SoundID.DOMINE_ASHES_TO_WIND,
        _clip:      "domine_ashes_to_wind",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class DomineWhyHere : Sound
{
    public DomineWhyHere() : base(
        _id:        SoundID.DOMINE_WHY_HERE,
        _clip:      "domine_why_here",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class DomineMountainSummit : Sound
{
    public DomineMountainSummit() : base(
        _id:        SoundID.DOMINE_MOUNTAIN_SUMMIT,
        _clip:      "domine_mountain_summit",
        _group:     AudioManager.voice_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 1f
    ){}
}

public class Electricity1 : Sound
{
    public Electricity1() : base(
        _id:        SoundID.ELECTRICITY_1,
        _clip:      "electricity_1",
        _group:     AudioManager.sfx_mixer,
        _volume:    0.5f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ){}
}

public class Electricity2 : Sound
{
    public Electricity2() : base(
        _id:        SoundID.ELECTRICITY_2,
        _clip:      "electricity_2",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ){}
}

public class Electricity3 : Sound
{
    public Electricity3() : base(
        _id:        SoundID.ELECTRICITY_3,
        _clip:      "electricity_3",
        _group:     AudioManager.sfx_mixer,
        _volume:    0.2f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ){}
}

public class BellChimes1 : Sound
{
    public BellChimes1() : base(
        _id:        SoundID.BELL_CHIMES,
        _clip:      "bell_chimes_1",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 0.55f,
        _min_pitch: 0.40f
    ){}
}

public class Thunder1 : Sound
{
    public Thunder1() : base(
        _id:        SoundID.THUNDER_1,
        _clip:      "thunder_1",
        _group:     AudioManager.sfx_mixer,
        _volume:    2f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ){}
}

public class Thunder2 : Sound
{
    public Thunder2() : base(
        _id:        SoundID.THUNDER_2,
        _clip:      "thunder_2",
        _group:     AudioManager.sfx_mixer,
        _volume:    2f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ){}
}


public class ThunderDistant : Sound
{
    public ThunderDistant() : base(
        _id:        SoundID.THUNDER_DISTANT,
        _clip:      "thunder_distant",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ){}
}

public class HeavyWind : Sound
{
    public HeavyWind() : base(
        _id:        SoundID.HEAVY_WIND,
        _clip:      "heavy_wind",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ){}
}

public class ElectricityLoop : Sound
{
    public ElectricityLoop() : base(
        _id:        SoundID.ELECTRICITY_LOOP,
        _clip:      "electricity_loop",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.84f
    ){}
}

public class ElectricBurst : Sound
{
    public ElectricBurst() : base(
        _id:        SoundID.ELECTRIC_BURST,
        _clip:      "electric_burst",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 0.9f,
        _min_pitch: 0.8f
    ){}
}

public class Ping : Sound
{
    public Ping() : base(
        _id:        SoundID.PING,
        _clip:      "ping",
        _group:     AudioManager.sfx_mixer,
        _volume:    0.5f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ){}
}

public class MagicFootstep1 : Sound
{
    public MagicFootstep1() : base(
        _id:        SoundID.MAGIC_FOOTSTEP_1,
        _clip:      "magic_footstep_1",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ){}
}

public class MagicFootstep2 : Sound
{
    public MagicFootstep2() : base(
        _id:        SoundID.MAGIC_FOOTSTEP_2,
        _clip:      "magic_footstep_2",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ){}
}

public class MagicFootstep3 : Sound
{
    public MagicFootstep3() : base(
        _id:        SoundID.MAGIC_FOOTSTEP_3,
        _clip:      "magic_footstep_3",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ){}
}

public class SnowRolling : Sound
{
    public SnowRolling() : base(
        _id:        SoundID.SNOW_ROLLING,
        _clip:      "snow_rolling",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.85f
    ){}
}

public class MagicSnowCast : Sound
{
    public MagicSnowCast() : base(
        _id:        SoundID.MAGIC_SNOW_CAST,
        _clip:      "magic_snow_cast",
        _group:     AudioManager.sfx_mixer,
        _volume:    1f,
        _max_pitch: 1f,
        _min_pitch: 0.8f
    ){}
}

}