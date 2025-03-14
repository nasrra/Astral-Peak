using System;
using System.Collections;
using System.Collections.Generic;
using Entropek;
using UnityEngine;

public static class DisplaySettingsManager{
    static Dictionary<int, Vector2Int> resolutions = new Dictionary<int, Vector2Int>(){
        {0, new Vector2Int(1280,720)},
        {1, new Vector2Int(1920,1080)},
        {2, new Vector2Int(2560,1440)},
        {3, new Vector2Int(3840,2160)},
    };
    static Dictionary<int, int> frame_caps = new Dictionary<int, int>(){
        {0, -1},
        {1, 30},
        {2, 60},
        {3, 120},
        {4, 165},
        {5, 244},
    };
    static int resolution_preset            = 0;
    static int frame_rate_preset            = 0;
    static int vsync_preset                 = 0;
    static bool fullscreen_preset           = false;
    static Coroutine change_buffer_coroutine;
    public static Action change_buffer_started, change_buffer_cancelled, change_buffer_accepted;
    public static readonly float change_buffer_time = 6f;

    public static void initialize(){
        load_player_prefs();
    }
    public static void load_player_prefs(){
        set_resolution(load_resolution_preset());
        set_fullscreen(load_fullscreen_preset());
        set_frame_rate_preset(load_frame_rate_preset());
        set_vsync_preset(load_vsync_preset());
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

    private static int load_vsync_preset(){
        return PlayerPrefs.GetInt("vsync_preset",vsync_preset);
    }

    public static void save_player_prefs(){
        // Log.MethodCall();
        PlayerPrefs.SetInt("frame_rate_preset",frame_rate_preset);
        PlayerPrefs.SetInt("fullscreen", fullscreen_preset == true?1:0);
        PlayerPrefs.SetInt("resolution_preset", resolution_preset);
        PlayerPrefs.SetInt("vsync_preset",vsync_preset);
        PlayerPrefs.Save();
    }

    public static void begin_resolution_change(int _preset){
        change_buffer_coroutine = UnityHook.instance.StartCoroutine(
            settings_change_buffer(
                _start_action:()=>{set_resolution(_preset);},
                _time_out:()=>{set_resolution(load_resolution_preset());}
            )
        );
    }

    public static void begin_fullscreen_change(bool _preset){
        change_buffer_coroutine = UnityHook.instance.StartCoroutine(
            settings_change_buffer(
                _start_action:()=>{set_fullscreen(_preset);},
                _time_out:()=>{set_fullscreen(load_fullscreen_preset());}
            )
        );    
    }

    private static void set_resolution(int _preset){
        resolution_preset = _preset;
        Vector2Int resolution = resolutions[_preset];
        Screen.SetResolution(resolution.x, resolution.y, fullscreen_preset);
    }
    private static void set_fullscreen(bool _fullscreen){
        fullscreen_preset = _fullscreen;
        Screen.fullScreenMode = fullscreen_preset == false? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
        set_resolution(resolution_preset);
    }
    public static void set_frame_rate_preset(int _preset){
        frame_rate_preset = _preset;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frame_caps[_preset];
    }
    public static void set_vsync_preset(int _preset){
        vsync_preset = _preset;
        QualitySettings.vSyncCount = _preset;
        // Debug.Log(QualitySettings.vSyncCount);
    }

    // used for confirmation when changing important settings.
    // so that the player doesnt softlock themselves from choosing a really high resolution.

    
    static IEnumerator settings_change_buffer(Action _start_action, Action _time_out){
        yield return Util.unscaled_timer(
            time: change_buffer_time,
            start_action:()=>{
                _start_action();
                change_buffer_started?.Invoke();
            },
            time_out:()=>{
                _time_out();
                change_buffer_cancelled?.Invoke();
            } 
        );
    }

    public static void accept_display_settings_change(){
        change_buffer_accepted?.Invoke();
        UnityHook.instance.StopCoroutine(change_buffer_coroutine);
    }

    public static void cancel_display_settings_change(){
        load_player_prefs();        
        change_buffer_cancelled?.Invoke();
        UnityHook.instance.StopCoroutine(change_buffer_coroutine);
    }

    public static int get_resolution_preset()   => resolution_preset;
    public static bool get_fullscreen_preset()  => fullscreen_preset;
    public static int get_frame_rate_preset()   => frame_rate_preset;
    public static int get_vsync_preset()        => vsync_preset;
}