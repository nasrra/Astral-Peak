using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossSpriteHandler : SpriteHandler{
    [SerializeField] Material
        hurt_material,
        death_material;

    void Awake() => set_material(hurt_material);
    public void play_damaged_flash() => state_switch(pulse_value("_amount", 1, 0.25f));
    public void play_death_effect(float time){
        set_material(death_material);
        state_switch(lerp_value("_amount", 2, 0, time));
    }
}
