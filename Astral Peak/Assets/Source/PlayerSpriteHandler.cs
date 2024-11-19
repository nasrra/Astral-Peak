public class PlayerSpriteHandler : SpriteHandler{
    public void play_damaged_flash() => state_switch(pulse_value("_amount",5,0.35f));
}
