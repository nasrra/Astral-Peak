using UnityEngine;

public class DisplayResolutionFullscreenToggle : MonoBehaviour{
    public void enable_fullscreen(bool toggle) => DisplaySettingsManager.set_windowed(toggle);
}
