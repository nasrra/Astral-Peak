using System.Collections;
using UnityEngine;

public class AstralPlaneRoomHandler : RoomHandler{
    [Header("AstralPlaneRoomHandler")]
    [field: SerializeField] public DomineDoor domine_door {get; private set;}
    [field: SerializeField] public CreditsHandler credits_handler {get; private set;}
    [field: SerializeField] public Domine domine {get; private set;}
    protected override void Start(){
        CameraEffects.instance.astral_plane_state();
        credits_handler.credits_ended += credits_ended;
        Health player_health = Player.instance.health;
        player_health.invulnerable();
        player_health.lock_state = true;
        UiManager.instance.lock_gameplay_ui_toggle = true;
        // credits_ended();
        base.Start();
    }

    public void end_credits_state(){
        domine_door.closed();
        credits_handler.start_credits();
    }

    void credits_ended(){
        StartCoroutine(credits_ended_coroutine());
    }

    IEnumerator credits_ended_coroutine(){
        yield return new WaitForSeconds(4);
        AudioManager.stop_music();
        yield return new WaitForSeconds(4);
        domine_door.open();
        yield break;
    }
}
