using System.Collections.Generic;
using UnityEngine;

public class BossSpriteHandler : SpriteHandler{
    Coroutine 
        dissolve_state,
        damaged_state,
        charge_state,
        background_state;
    public void play_damaged_flash() => state_switch(ref damaged_state, pulse_value("_damaged_amount", 1f, 0f, .25f, 1));
    public void play_death_effect(float time)           => state_switch(ref dissolve_state,lerp_value("_dissolve_amount", 1, 0, time));
    public void play_death_effect_reverse(float time)   => state_switch(ref dissolve_state, lerp_value("_dissolve_amount", 0, 1, time));
    public void play_charged_flash()                    => state_switch(ref charge_state, pulse_value("_charge_amount", 1f, 0f, 1f, 1));
    public void play_charged_flash(string sprite_id)    => state_switch(ref charge_state, pulse_value(sprite_id, "_charge_amount", 1f, 0f, 1f, 1));
    public void play_charged_flash(List<string> sprite_id)    => state_switch(ref charge_state, pulse_value(sprite_id, "_charge_amount", 1f, 0f, 1f, 1));
    public void enter_background(float time) => state_switch(ref background_state, lerp_value("_background_amount", 0, 1, time));
    public void exit_background(float time) => state_switch(ref background_state, lerp_value("_background_amount", 1, 0, time));
    public void set_background(bool _in_background) => set_value("_background_amount",_in_background?1:0);
    public void renew(){
        StopAllCoroutines();
        set_value("_dissolve_amount",1);
        set_value("_charge_amount",0);
        set_value("_background_amount",0);
    }
    public void lerp_charged_amount(float amount, float time) => state_switch(ref charge_state, lerp_value("_charge_amount",0,amount,time));
}
