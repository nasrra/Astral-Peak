using UnityEditor;
using UnityEngine;

public static class DisplayResolutionManager{
    static Vector2Int resolution = new Vector2Int(1920,1080);
    static bool windowed = true;
    public static void initialize(){
    update_resolution();
    }
    public static void set_resolution(Vector2Int _resolution){
        resolution = _resolution;
        update_resolution();
    }
    public static void set_windowed(bool _windowed){
        windowed = _windowed;
        update_resolution();
    }
    private static void update_resolution() => Screen.SetResolution(resolution.x, resolution.y, windowed);
}
