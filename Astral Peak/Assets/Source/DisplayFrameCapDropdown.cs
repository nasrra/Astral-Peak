using UnityEngine;
using static TMPro.TMP_Dropdown;

public class DisplayFrameRateDropdown : DisplaySettingsDropDown{
    protected override int load_value(){
        return DisplaySettingsManager.get_frame_rate_preset();
    }

    protected override void on_value_change(int _selection){
        DisplaySettingsManager.set_frame_rate_preset(_selection);
    }
}
