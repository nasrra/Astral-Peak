using UnityEngine;

public class PlayerSpriteHandler : SpriteHandler{
    Coroutine state;

    public void play_damaged_flash() => state_switch(ref state, pulse_value("_damaged_amount",5,.5f));
    public void play_death_effect(float time) => state_switch(ref state, lerp_value("_dissolve_amount", 1, 0, time));
}
