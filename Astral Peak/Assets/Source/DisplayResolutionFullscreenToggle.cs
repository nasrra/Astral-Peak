using UnityEngine;

public class DisplayResolutionFullscreenToggle : MonoBehaviour{
    public void enable_fullscreen(bool toggle) => DisplayResolutionManager.set_windowed(toggle);
}
