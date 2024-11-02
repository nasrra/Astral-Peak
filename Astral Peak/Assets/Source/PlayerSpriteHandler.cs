public class PlayerSpriteHandler : SpriteHandler{
    public override void play_damaged_flash() => switch_state(damaged_flash(6,0.33f));
}
