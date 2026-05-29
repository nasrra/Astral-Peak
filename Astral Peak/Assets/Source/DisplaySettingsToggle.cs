using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Toggle;

public abstract class DisplaySettingsToggle : MonoBehaviour{    
    [SerializeField] Toggle toggle;
    ToggleEvent toggle_event;
    protected abstract bool load_value();
    protected abstract void on_value_change(bool x);

    protected virtual void OnEnable(){
        toggle.isOn = load_value();
        toggle_event = toggle.onValueChanged;
        toggle_event.AddListener(on_value_change);
        DisplaySettingsManager.change_buffer_cancelled += refresh_value;
    }
    public void refresh_value(){
        toggle_event.RemoveListener(on_value_change);
        toggle.isOn = load_value();
        toggle_event.AddListener(on_value_change);
    }
    protected virtual void OnDisable(){
        toggle_event.RemoveListener(on_value_change);        
        DisplaySettingsManager.change_buffer_cancelled -= refresh_value;
    }
}
