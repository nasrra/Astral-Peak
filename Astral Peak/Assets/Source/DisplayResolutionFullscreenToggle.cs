using UnityEngine;
using UnityEngine.UI;

public class DisplayResolutionFullscreenToggle : DisplaySettingsToggle
{
    protected override bool load_value(){
        return DisplaySettingsManager.get_fullscreen_preset();
    }

    protected override void on_value_change(bool _selected){
        DisplaySettingsManager.begin_fullscreen_change(_selected);
    }
}