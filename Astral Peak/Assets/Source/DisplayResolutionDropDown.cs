using TMPro;
using UnityEngine;

public class DisplayResolutionDropDown : MonoBehaviour{
    [SerializeField] TMP_Dropdown dropdown;
    void OnEnable() => dropdown.value = DisplaySettingsManager.get_resolution_preset();
    public void resolution_changed(int selection) => DisplaySettingsManager.begin_resolution_change(selection);
}
