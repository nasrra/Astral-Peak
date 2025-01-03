using UnityEngine;

public class WeaponSpriteHandler : SpriteHandler{
    Coroutine state;
    public void play_damaged_flash() => state_switch(ref state, pulse_value("hurt_flash_amount", 1, .25f));
    public void play_charged_flash() => state_switch(ref state, pulse_value("charge_amount", 1, 1f));
}
