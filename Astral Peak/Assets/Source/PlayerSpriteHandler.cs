using UnityEngine;

public class PlayerSpriteHandler : SpriteHandler{
    Coroutine state;
    public void play_damaged_flash() => state_switch(ref state, pulse_value("_damaged_amount",1f,0f,.5f,5));
    public void play_death_effect(float time) => state_switch(ref state, lerp_value("_dissolve_amount", 1, 0, time));
    public void fade_to_black() => state_switch(ref state, lerp_value("_domine_door_amount", 0, 1, 4));
    public void fade_from_black() => state_switch(ref state, lerp_value("_domine_door_amount", 1, 0, 4));
    public void set_black() => set_value("_domine_door_amount", 1);
    public void enter_domine_door_layer(){
        set_sorting_layer("body",SortingLayerManager.BACKGROUND, -2);
        set_sorting_layer("cape",SortingLayerManager.BACKGROUND, -3);
        set_sorting_layer("sword_holstered",SortingLayerManager.BACKGROUND, -4);
    }
    public void enter_characters_layer(){
        set_sorting_layer("body" ,SortingLayerManager.CHARACTERS, 0);
        set_sorting_layer("cape",SortingLayerManager.CHARACTERS, -1);        
        set_sorting_layer("sword_holstered",SortingLayerManager.CHARACTERS, -2);
    }
}
