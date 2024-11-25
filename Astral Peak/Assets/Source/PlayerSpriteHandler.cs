using UnityEngine;

public class PlayerSpriteHandler : SpriteHandler{
    [SerializeField] Material
        hurt_material,
        death_material;

    public void play_damaged_flash() => state_switch(pulse_value("_amount",5,0.35f));
    public void play_death_effect(float time){
        set_material(death_material);
        state_switch(lerp_value("_amount", 2, 0, time));
    }
}
