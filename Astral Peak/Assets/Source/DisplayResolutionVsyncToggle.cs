using UnityEngine;

public class DisplayResolutionVsyncToggle : DisplaySettingsToggle{
    protected override bool load_value(){
        return DisplaySettingsManager.get_vsync_preset() == 1? true : false;
    }

    protected override void on_value_change(bool x){
        DisplaySettingsManager.set_vsync_preset(x==true?1:0);
    }
}
