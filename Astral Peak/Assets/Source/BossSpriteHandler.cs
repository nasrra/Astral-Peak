using UnityEngine;

public class BossSpriteHandler : SpriteHandler{
    [SerializeField] Material
        hurt_material,
        death_material;
    Coroutine state;

    void Awake() => set_material(hurt_material);
    public void play_damaged_flash(){
        set_material(hurt_material);
        state_switch(ref state, pulse_value("_amount", 1, .25f));
    }
    public void play_death_effect(float time){
        set_material(death_material);
        state_switch(ref state,lerp_value("_amount", 2, 0, time));
    }
    public void play_death_effect_reverse(float time){
        set_material(death_material);
        state_switch(ref state, lerp_value("_amount", 0, 2, time));
    }
}
