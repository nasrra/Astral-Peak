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
    static int resolution_preset    = 0;
    static int frame_rate_preset    = 0;
    static bool fullscreen          = false;
    static Coroutine change_buffer_coroutine;
    static Action change_buffer_started, change_buffer_completed;

    public static void initialize(){
        load_player_prefs();
    }
    public static void load_player_prefs(){
        set_resolution(load_resolution_preset());
        set_fullscreen(load_fullscreen_preset());
        set_frame_rate_preset(load_frame_rate_preset());
    }

    private static int load_resolution_preset(){
        return PlayerPrefs.GetInt("resolution_preset", resolution_preset);
    }

    private static bool load_fullscreen_preset(){
        return PlayerPrefs.GetInt("fullscreen",0) == 1? true: false;
    }

    private static int load_frame_rate_preset(){
        return PlayerPrefs.GetInt("frame_rate_preset",frame_rate_preset);
    }

    public static void save_player_prefs(){
        PlayerPrefs.SetInt("frame_cap_preset",frame_rate_preset);
        PlayerPrefs.SetInt("fullscreen", fullscreen == true?1:0);
        PlayerPrefs.SetInt("resolution_preset", resolution_preset);
    }

    public static void begin_resolution_change(int _preset){
        change_buffer_coroutine = UnityHook.instance.StartCoroutine(resolution_changed_buffer(_preset, load_resolution_preset()));
    }

    public static void begin_fullscreen_change(bool _preset){
        change_buffer_coroutine = UnityHook.instance.StartCoroutine(fullscreen_changed_buffer(_preset, load_fullscreen_preset()));
    }

    private static void set_resolution(int preset){
        resolution_preset = preset;
        Vector2Int resolution = resolutions[preset];
        Screen.SetResolution(resolution.x, resolution.y, fullscreen);
    }
    private static void set_fullscreen(bool _fullscreen){
        fullscreen = _fullscreen;
        Screen.fullScreenMode = fullscreen == false? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
        //set_resolution(resolution_preset);
    }
    public static void set_frame_rate_preset(int preset){
        frame_rate_preset = preset;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frame_caps[preset];
        save_player_prefs();
    }

    // used for confirmation when changing important settings.
    // so that the player doesnt softlock themselves from choosing a really high resolution.

    
    static IEnumerator resolution_changed_buffer(int _selected_preset, int _previous_preset){
        yield return Util.unscaled_timer(
            time: 6,
            start_action:()=>{
                set_resolution(_selected_preset);
                change_buffer_started?.Invoke();
            },
            time_out:()=>{
                set_resolution(_previous_preset);
                change_buffer_completed?.Invoke();
            } 
        );
    }

    static IEnumerator fullscreen_changed_buffer(bool _selected_preset, bool _previous_preset){
        yield return Util.unscaled_timer(
            time: 6,
            start_action:()=>{
                set_fullscreen(_selected_preset);
                change_buffer_started?.Invoke();
            },
            time_out:()=>{
                set_fullscreen(_previous_preset);
                change_buffer_completed?.Invoke();
            }
        );
    }

    public static void stop_display_settings_change_buffer(){
        save_player_prefs();
        UnityHook.instance.StopCoroutine(change_buffer_coroutine);
    }


    public static int get_resolution_preset()   => resolution_preset;
    public static bool get_is_fullscreen()      => fullscreen;
    public static int get_frame_rate_preset()   => frame_rate_preset;

}