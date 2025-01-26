using System;
using System.Collections.Generic;
using UnityEngine;

public class DisplayResolutionDropDown : MonoBehaviour{
    Dictionary<int, Action> resolutions = new Dictionary<int, Action>(){
        {0, ()=>DisplayResolutionManager.set_resolution(new Vector2Int(1920,1080))},
        {1, ()=>DisplayResolutionManager.set_resolution(new Vector2Int(2560,1440))},
        {2, ()=>DisplayResolutionManager.set_resolution(new Vector2Int(3840,2160))},
    };
    public void resolution_changed(int selection) => resolutions[selection]();
}
