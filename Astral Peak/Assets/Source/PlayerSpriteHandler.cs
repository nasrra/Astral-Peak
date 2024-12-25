using UnityEngine;

public class PlayerSpriteHandler : SpriteHandler{
    [SerializeField] Material
        hurt_material,
        death_material;
    Coroutine state;

    public void play_damaged_flash() => state_switch(ref state, pulse_value("_amount",5,.5f));
    public void play_death_effect(float time){
        set_material(death_material);
        state_switch(ref state, lerp_value("_amount", 2, 0, time));
    }
}
