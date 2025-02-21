using UnityEngine;

public class DisplayFrameCapDropdown : MonoBehaviour{
    [SerializeField] TMPro.TMP_Dropdown dropdown;
    void OnEnable() => dropdown.value = DisplaySettingsManager.get_frame_rate_preset();
    public void frame_cap_changed(int selection) => DisplaySettingsManager.set_frame_rate_preset(selection);
}
//