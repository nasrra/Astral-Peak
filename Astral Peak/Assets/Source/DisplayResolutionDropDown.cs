using TMPro;
using UnityEngine;
using static TMPro.TMP_Dropdown;

public class DisplayResolutionDropDown : DisplaySettingsDropDown{
    protected override int load_value(){
        return DisplaySettingsManager.load_resolution_preset();
    }

    protected override void on_value_change(int _selection){
        DisplaySettingsManager.begin_resolution_change(_selection);
    }
}
