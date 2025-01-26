using UnityEngine;
using UnityEngine.UI;

public class DisplayResolutionFullscreenToggle : MonoBehaviour{
    [SerializeField] Toggle toggle;
    void OnEnable() => toggle.isOn = PlayerPrefs.GetInt("fullscreen",0) == 1? true : false;
    public void enable_fullscreen(bool toggle) => DisplaySettingsManager.set_fullscreen(toggle);
}