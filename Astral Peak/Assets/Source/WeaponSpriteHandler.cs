using UnityEngine;

public class WeaponSpriteHandler : SpriteHandler{
    Coroutine state;
    public void play_damaged_flash() => state_switch(ref state, pulse_value("hurt_flash_amount", 1f,0f,0.25f, 1));
    public void play_charged_flash() => state_switch(ref state, pulse_value("charge_amount", 1f,0f,1f,1));
}
