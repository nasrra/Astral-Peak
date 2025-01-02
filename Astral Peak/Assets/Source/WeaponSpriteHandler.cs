using UnityEngine;

public class WeaponSpriteHandler : SpriteHandler{
    [SerializeField] Material flash_material;
    Coroutine state;
    public void play_damaged_flash(){
        set_material(flash_material);
        state_switch(ref state, pulse_value("_amount", 1, .25f));
    }
}
