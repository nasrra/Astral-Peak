using System.Collections;
using UnityEngine;

public class AstralPlaneRoomHandler : RoomHandler{
    [Header("AstralPlaneRoomHandler")]
    [SerializeField] DomineDoor domine_door;
    [SerializeField] CreditsHandler credits_handler;
    [field: SerializeField] public Domine domine {get; private set;}
    protected override void Start(){
        CameraEffects.instance.astral_plane_state();
        credits_handler.credits_ended += credits_ended;
        Health player_health = Player.instance.health;
        player_health.invulnerable();
        player_health.lock_state = true;
        credits_ended();
        base.Start();
    }

    public DomineDoor get_domine_door() => domine_door;

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
