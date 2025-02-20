using System;
using System.Collections;
using System.Collections.Generic;
using Entropek;
using UnityEngine;

public static class DisplaySettingsManager{
    static Dictionary<int, Vector2Int> resolutions = new Dictionary<int, Vector2Int>(){
        {0, new Vector2Int(1920,1080)},
        {1, new Vector2Int(2560,1440)},
        {2, new Vector2Int(3840,2160)},
    };
    static Dictionary<int, int> frame_caps = new Dictionary<int, int>(){
        {0, -1},
        {1, 30},
        {2, 60},
        {3, 120},
        {4, 165},
        {5, 244},
    };
    static int resolution_preset = 0;
    static bool fullscreen = false;
    static int frame_cap_preset = 0;
    static Coroutine change_buffer_coroutine;
    static Action change_buffer_started, change_buffer_completed;

    public static void initialize(){
        load_player_prefs();
        set_resolution(resolution_preset);
        set_fullscreen(fullscreen);
        set_frame_cap_preset(frame_cap_preset);
    }
    public static void load_player_prefs(){
        resolution_preset   = PlayerPrefs.GetInt("resolution_preset", resolution_preset);
        fullscreen          = PlayerPrefs.GetInt("fullscreen",0) == 1? true: false;
        frame_cap_preset    = PlayerPrefs.GetInt("frame_cap_preset",frame_cap_preset);
    }
    public static void set_resolution(int preset){
        resolution_preset = preset;
        Vector2Int resolution = resolutions[preset];
        Screen.SetResolution(resolution.x, resolution.y, fullscreen);
        PlayerPrefs.SetInt("resolution_preset", resolution_preset);
    }
    public static void set_fullscreen(bool _fullscreen){
        fullscreen = _fullscreen;
        Screen.fullScreenMode = fullscreen == false? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
        //set_resolution(resolution_preset);
        PlayerPrefs.SetInt("fullscreen", _fullscreen == true?1:0);
    }
    public static void set_frame_cap_preset(int preset){
        frame_cap_preset = preset;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frame_caps[preset];
        PlayerPrefs.SetInt("frame_cap_preset",preset);
    }

    // used for confirmation when changing important settings.
    // so that the player doesnt softlock themselves from choosing a really high resolution.
    static void start_display_settings_change_buffer(DisplaySettings _previous_settings, DisplaySettings _selected_settings){
        change_buffer_coroutine = UnityHook.instance.StartCoroutine(display_settings_changed_buffer(_previous_settings, _selected_settings));
    }
    static IEnumerator display_settings_changed_buffer(DisplaySettings _previous_settings, DisplaySettings _selected_settings){
        yield return Util.unscaled_timer(
            time: 6,
            start_action:()=>{
                set_resolution(_selected_settings.resolution_preset);
                set_fullscreen(_selected_settings.fullscreen_preset);
                change_buffer_started?.Invoke();
            },
            time_out:()=>{
                set_resolution(_previous_settings.resolution_preset);
                set_fullscreen(_previous_settings.fullscreen_preset);
                change_buffer_completed?.Invoke();
            } 
        );
    }
    public static void stop_display_settings_change_buffer(){
        UnityHook.instance.StopCoroutine(change_buffer_coroutine);
    }

    public static int get_resolution_preset()   => resolution_preset;
    public static bool get_is_fullscreen()      => fullscreen;
    public static int get_frame_cap_preset()    => frame_cap_preset;

    struct DisplaySettings{
        public readonly int resolution_preset;
        public readonly bool fullscreen_preset;
        public DisplaySettings(int _resolsution_preset, bool _fullscreen_preset){
            this.resolution_preset = _resolsution_preset;
            this.fullscreen_preset = _fullscreen_preset;
        }
    }

}