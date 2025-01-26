using UnityEngine;

public static class DisplaySettingsManager{
    static Vector2Int resolution = new Vector2Int(1920,1080);
    static int display = 0;
    static bool windowed = true;
    public static void initialize(){
        set_resolution(resolution);
        set_windowed(windowed);
        set_display(display);
    }
    public static void set_resolution(Vector2Int _resolution){
        resolution = _resolution;
        Screen.SetResolution(resolution.x, resolution.y, windowed);
    }
    public static void set_windowed(bool _windowed){
        windowed = _windowed;
        Screen.fullScreenMode = windowed == true? FullScreenMode.Windowed : FullScreenMode.ExclusiveFullScreen;
    }
    public static void set_display(int _display){
        display = _display;
        Display.displays[_display].Activate();
    }
}
