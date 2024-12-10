using System.Collections.Generic;
using Sounds;

public interface SceneSounds{
    public List<Sound> get_sounds();
}


public struct SnowForestSceneSounds : SceneSounds{
    public List<Sound> get_sounds(){
        List<Sound> ids = new List<Sound>();
        foreach(SoundSet set in sets)
            ids.AddRange(set.get_sounds());
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
    public List<Sound> get_sounds(){
        List<Sound> ids = new List<Sound>();
        foreach(SoundSet set in sets)
            ids.AddRange(set.get_sounds());
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
    public List<Sound> get_sounds(){
        List<Sound> ids = new List<Sound>();
        foreach(SoundSet set in sets)
            ids.AddRange(set.get_sounds());
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
    public List<Sound> get_sounds(){
        List<Sound> ids = new List<Sound>();
        foreach(SoundSet set in sets)
            ids.AddRange(set.get_sounds());
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
    public List<Sound> get_sounds(){
        List<Sound> ids = new List<Sound>();
        foreach(SoundSet set in sets)
            ids.AddRange(set.get_sounds());
        return ids;
    }
    List<SoundSet> sets => new List<SoundSet>(){
        new UiSoundSet(),
    };
}