using UnityEngine;

public class AstralPlaneRoomHandler : RoomHandler{
    [Header("AstralPlaneRoomHandler")]
    [SerializeField] Animator credits;
    protected override void Start(){
        CameraEffects.instance.astral_plane_state();
        base.Start();
    }

    public void play_end_credits(){
        credits.Play("CreditsLoop");
    }
}
