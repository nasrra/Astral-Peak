using System.Collections.Generic;
using Sounds;
using UnityEditor.SearchService;
using UnityEngine.InputSystem.LowLevel;

namespace SceneSounds{

public interface SceneSoundSet{
    public List<Sound> get_sounds();
}


public struct SnowForest : SceneSoundSet{
    public List<Sound> get_sounds(){
        List<Sound> sounds = new List<Sound>();
        foreach(SoundSet set in sets)
            sounds.AddRange(set.get_sounds());
        return sounds;
    }
    List<SoundSet> sets => new List<SoundSet>(){
        new MagicSoundSet(),
        new UiSoundSet(),
        new OutdoorAmbienceSoundSet(),
        new MeleeSoundSet(),
        new SnowSoundSet(),
        new DomineSoundSet(),
        new StoneSoundSet(),
    };
}

public struct WolfBossRoom : SceneSoundSet{
    public List<Sound> get_sounds(){
        List<Sound> sounds = new List<Sound>();
        foreach(SoundSet set in sets)
            sounds.AddRange(set.get_sounds());
        return sounds;
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

public struct TutorialRoom : SceneSoundSet{
    public List<Sound> get_sounds(){
        List<Sound> sounds = new List<Sound>();
        foreach(SoundSet set in sets)
            sounds.AddRange(set.get_sounds());
        return sounds;
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

public struct Shrine : SceneSoundSet{
    public List<Sound> get_sounds(){
        List<Sound> sounds = new List<Sound>();
        foreach(SoundSet set in sets)
            sounds.AddRange(set.get_sounds());
        return sounds;
    }
    List<SoundSet> sets => new List<SoundSet>(){
        new MagicSoundSet(),
        new UiSoundSet(),
        new OutdoorAmbienceSoundSet(),
        new MeleeSoundSet(),
        new StoneSoundSet(),
        new DomineSoundSet(),
        new FireSoundSet(),
        new AltarCutsceneSoundSet(),
        new BeatriceSoundSet(),
    };
}

public struct MainMenu : SceneSoundSet{
    public List<Sound> get_sounds(){
        List<Sound> sounds = new List<Sound>();
        foreach(SoundSet set in sets)
            sounds.AddRange(set.get_sounds());
        return sounds;
    }
    List<SoundSet> sets => new List<SoundSet>(){
        new UiSoundSet(),
    };
}

public struct Tower1 : SceneSoundSet{
    public List<Sound> get_sounds(){
        List<Sound> sounds = new List<Sound>();
        foreach(SoundSet set in sets)
            sounds.AddRange(set.get_sounds());
        return sounds;
    }
    List<SoundSet> sets => new List<SoundSet>(){
        new MagicSoundSet(),
        new UiSoundSet(),
        new OutdoorAmbienceSoundSet(),
        new MeleeSoundSet(),
        new StoneSoundSet(),
        new HollowSoundSet(),
        new FireSoundSet(),
    };
}

public struct MageBossRoom : SceneSoundSet{
    public List<Sound> get_sounds(){
        List<Sound> sounds = new List<Sound>();
        foreach(SoundSet set in sets)
            sounds.AddRange(set.get_sounds());
        return sounds;
    }
    List<SoundSet> sets => new List<SoundSet>(){
        new MagicSoundSet(),
        new UiSoundSet(),
        new OutdoorAmbienceSoundSet(),
        new MeleeSoundSet(),
        new SnowSoundSet(),
        new HollowSoundSet(),
        new MageSoundSet(),
        new ThunderSoundSet(),
        new ElectricitySoundSet(),
    };
}

public struct SnowField : SceneSoundSet{
    public List<Sound> get_sounds(){
        List<Sound> sounds = new List<Sound>();
        foreach(SoundSet set in sets)
            sounds.AddRange(set.get_sounds());
        return sounds;
    }
    List<SoundSet> sets => new List<SoundSet>(){
        new MagicSoundSet(),
        new UiSoundSet(),
        new OutdoorAmbienceSoundSet(),
        new MeleeSoundSet(),
        new SnowSoundSet(),
    };
}

public struct Introduction : SceneSoundSet{
    public List<Sound> get_sounds(){
        List<Sound> sounds = new List<Sound>();
        foreach(SoundSet set in sets)
            sounds.AddRange(set.get_sounds());
        return sounds;
    }
    List<SoundSet> sets => new(){
        new IntroductionSoundSet(),
    };
}

public struct FinalBossRoom : SceneSoundSet{
    public List<Sound> get_sounds(){
        List<Sound> sounds =new List<Sound>();
        foreach(SoundSet set in sets)
            sounds.AddRange(set.get_sounds());
        return sounds;
    }
    List<SoundSet> sets => new(){
        new UiSoundSet(),
        new StoneSoundSet(),
        new MagicSoundSet(),
        new MeleeSoundSet(),
        new OutdoorAmbienceSoundSet(),
        new GiantSoundSet(),
    };
}

public struct AstralPlane : SceneSoundSet{
    public List<Sound> get_sounds(){
        List<Sound> sounds =new List<Sound>();
        foreach(SoundSet set in sets)
            sounds.AddRange(set.get_sounds());
        return sounds;
    }
    List<SoundSet> sets => new(){
        new UiSoundSet(),
        new StoneSoundSet(),
        new MagicSoundSet(),
        new MeleeSoundSet(),
        new DomineSoundSet(),
    };
}

}