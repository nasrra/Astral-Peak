using UnityEngine;
using UnityEngine.UI;

public class DisplayResolutionFullscreenToggle : MonoBehaviour{
    [SerializeField] Toggle toggle;
    void OnEnable() => toggle.isOn = DisplaySettingsManager.get_is_fullscreen();
    public void enable_fullscreen(bool toggle) => DisplaySettingsManager.set_fullscreen(toggle);
}