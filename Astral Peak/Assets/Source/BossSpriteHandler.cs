using UnityEngine;

public class BossSpriteHandler : SpriteHandler{
    Coroutine 
        dissolve_state,
        damaged_state,
        charge_state;
    public void play_damaged_flash()                    => state_switch(ref damaged_state, pulse_value("_damaged_amount", 1, .25f));
    public void play_death_effect(float time)           => state_switch(ref dissolve_state,lerp_value("_dissolve_amount", 1, 0, time));
    public void play_death_effect_reverse(float time)   => state_switch(ref dissolve_state, lerp_value("_dissolve_amount", 0, 1, time));
    public void play_charged_flash()                    => state_switch(ref charge_state, pulse_value("_charge_amount", 1, 1f));
    public void play_charged_flash(string sprite_id)    => state_switch(ref charge_state, pulse_value(sprite_id, "_charge_amount", 1, 1f));
}
