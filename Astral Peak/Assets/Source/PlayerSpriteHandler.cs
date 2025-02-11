using UnityEngine;

public class PlayerSpriteHandler : SpriteHandler{
    Coroutine state;
    public void play_damaged_flash() => state_switch(ref state, pulse_value("_damaged_amount",5,.5f));
    public void play_death_effect(float time) => state_switch(ref state, lerp_value("_dissolve_amount", 1, 0, time));
    public void fade_to_black() => state_switch(ref state, lerp_color("_color", new Color(0,0,0,0),Color.black, 4));
    public void fade_from_black() => state_switch(ref state, lerp_color("_color", Color.black,new Color(0,0,0,0), 4));
    public void set_black() => set_color("_color", Color.black);
    public void enter_domine_door_layer(){
        set_sorting_layer("sword",SortingLayerManager.BACKGROUND, -2);
        set_sorting_layer("body",SortingLayerManager.BACKGROUND, -2);
        set_sorting_layer("cape",SortingLayerManager.BACKGROUND, -3);
    }
    public void enter_characters_layer(){
        set_sorting_layer("sword" ,SortingLayerManager.CHARACTERS, 1);
        set_sorting_layer("body" ,SortingLayerManager.CHARACTERS, 0);
        set_sorting_layer("cape",SortingLayerManager.CHARACTERS, -1);        
    }
}
