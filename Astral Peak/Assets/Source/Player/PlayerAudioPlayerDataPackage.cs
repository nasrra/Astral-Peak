using System.Collections.Generic;

public struct PlayerAudioPlayerDataPackage :  AudioPlayerEventDataPackage{
    public List<AudioPlayerEventData> get_event_instances(){
        return new List<AudioPlayerEventData>(){
        };
    }

    public List<AudioPlayerEventData> get_event_references(){
        return new List<AudioPlayerEventData>(){
            new(FMODEventFolders.SFX_SNOW,      "snow_footstep"),
            new(FMODEventFolders.SFX_SNOW,      "snow_impact_light"),
            new(FMODEventFolders.SFX_STONE,     "stone_footstep"),
            new(FMODEventFolders.SFX_STONE,     "stone_impact_light"),
            new(FMODEventFolders.SFX_MELEE,     "melee_swing_1"),
            new(FMODEventFolders.SFX_MELEE,     "melee_hit"),
            new(FMODEventFolders.SFX_MOVEMENT,  "dash_1"),
        };
    }
}
