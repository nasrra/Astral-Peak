using System.Collections.Generic;
using System.Net.Mail;
using UnityEditor.SearchService;

public interface SceneSounds{
    public List<SoundID> get_sound_ids();
}


public struct SnowForestSceneSounds : SceneSounds{
    public List<SoundID> get_sound_ids(){
        List<SoundID> ids = new List<SoundID>();
        foreach(SoundSet set in sets)
            ids.AddRange(set.get_sound_ids());
        return ids;
    }

    List<SoundSet> sets => new List<SoundSet>(){
        new MagicSoundSet(),
        new UiSoundSet(),
        new OutdoorAmbienceSoundSet(),
        new MeleeSoundSet(),
        new SnowSoundSet(),
        new DomineSoundSet()
    };
}

public struct WolfBossRoomSceneSounds : SceneSounds{
    public List<SoundID> get_sound_ids(){
        List<SoundID> ids = new List<SoundID>();
        foreach(SoundSet set in sets)
            ids.AddRange(set.get_sound_ids());
        return ids;
    }
    List<SoundSet> sets => new List<SoundSet>(){
        new WolfSoundSet(),
        new MagicSoundSet(),
        new UiSoundSet(),
        new FireSoundSet(),
        new OutdoorAmbienceSoundSet(),
        new RangedSoundSet(),
        new MeleeSoundSet(),
        new SnowSoundSet(),
        new CavalrySoundSet(),
        new RiderSoundSet(),
    };
}

public struct TutorialRoomSceneSounds : SceneSounds{
    public List<SoundID> get_sound_ids(){
        List<SoundID> ids = new List<SoundID>();
        foreach(SoundSet set in sets)
            ids.AddRange(set.get_sound_ids());
        return ids;
    }
    List<SoundSet> sets => new List<SoundSet>(){
        new MagicSoundSet(),
        new UiSoundSet(),
        new OutdoorAmbienceSoundSet(),
        new MeleeSoundSet(),
        new StoneSoundSet(),
        new DomineSoundSet(),
        new HollowSoundSet(),
        new FireSoundSet(),
    };
}

public struct ShrineSceneSounds : SceneSounds{
    public List<SoundID> get_sound_ids(){
        List<SoundID> ids = new List<SoundID>();
        foreach(SoundSet set in sets)
            ids.AddRange(set.get_sound_ids());
        return ids;
    }
    List<SoundSet> sets => new List<SoundSet>(){
        new MagicSoundSet(),
        new UiSoundSet(),
        new OutdoorAmbienceSoundSet(),
        new MeleeSoundSet(),
        new StoneSoundSet(),
        new DomineSoundSet(),
        new FireSoundSet(),
        new AltarCutsceneSoundSet()
    };
}

public struct MainMenuSceneSounds : SceneSounds{
    public List<SoundID> get_sound_ids(){
        List<SoundID> ids = new List<SoundID>();
        foreach(SoundSet set in sets)
            ids.AddRange(set.get_sound_ids());
        return ids;
    }
    List<SoundSet> sets => new List<SoundSet>(){
        new UiSoundSet(),
    };
}

public struct TempSceneSounds : SceneSounds{
    public List<SoundID> get_sound_ids(){
        List<SoundID> ids = new List<SoundID>();
        foreach(SoundSet set in sets)
            ids.AddRange(set.get_sound_ids());
        return ids;
    }
    List<SoundSet> sets => new List<SoundSet>(){
        new UiSoundSet(),
    };
}