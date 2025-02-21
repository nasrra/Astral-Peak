using System;
using TMPro;
using UnityEngine;
using static TMPro.TMP_Dropdown;

public abstract class DisplaySettingsDropDown : MonoBehaviour{
    [SerializeField] TMP_Dropdown dropdown;
    DropdownEvent dropdown_event;
    protected abstract int load_value();
    protected abstract void on_value_change(int x);

    protected virtual void OnEnable(){
        dropdown.value = load_value();
        dropdown_event = dropdown.onValueChanged;
        dropdown_event.AddListener(on_value_change);
        DisplaySettingsManager.change_buffer_cancelled += refresh_value;
    }
    public void refresh_value(){
        dropdown_event.RemoveListener(on_value_change);
        dropdown.value = load_value();
        dropdown_event.AddListener(on_value_change);
    }
    protected virtual void OnDisable(){
        dropdown_event.RemoveListener(on_value_change);        
        DisplaySettingsManager.change_buffer_cancelled -= refresh_value;
    }
}
