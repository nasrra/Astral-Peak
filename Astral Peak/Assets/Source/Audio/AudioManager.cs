using FMOD.Studio;
using FMODUnity;

public static class AudioManager{
    public static EventReference get_event_reference(string event_name_path)    => RuntimeManager.PathToEventReference(event_name_path);    
    public static EventInstance get_event_instance(string event_name_path)      => RuntimeManager.CreateInstance(get_event_reference(event_name_path));    
    
    
    public static EventReference get_sfx_event_reference(string event_name)    => RuntimeManager.PathToEventReference($"{FMODEventFolders.SFX}{event_name}");
    public static EventReference get_music_event_reference(string event_name)  => RuntimeManager.PathToEventReference($"{FMODEventFolders.MUSIC}{event_name}");
    public static EventReference get_voice_event_reference(string event_name)  => RuntimeManager.PathToEventReference($"{FMODEventFolders.VOICE}{event_name}");
    public static EventInstance get_sfx_event_instance(string event_name)      => RuntimeManager.CreateInstance(get_sfx_event_reference(event_name));
    public static EventInstance get_music_event_instance(string event_name)    => RuntimeManager.CreateInstance(get_music_event_reference(event_name));
    public static EventInstance get_voice_event_instance(string event_name)    => RuntimeManager.CreateInstance(get_voice_event_reference(event_name));
}

public static class FMODEventFolders{
    public const string 
        SFX             = "event:/sfx/",
        MUSIC           = "event:/music/",
        VOICE           = "event:/voice/",
        SFX_SNOW        = SFX+"snow/",
        SFX_STONE       = SFX+"stone/",
        SFX_MELEE       = SFX+"melee/",
        SFX_MOVEMENT    = SFX+"movement/",
        VOICE_DOMINE    = VOICE+"domine/"
    ;

}