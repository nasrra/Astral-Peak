using System.Collections;
using UnityEngine;

public class AstralPlaneRoomHandler : RoomHandler{
    [Header("AstralPlaneRoomHandler")]
    [SerializeField] DomineDoor domine_door;
    [SerializeField] Animator credits;
    [field: SerializeField] public Domine domine {get; private set;}
    protected override void Start(){
        CameraEffects.instance.astral_plane_state();
        base.Start();
    }

    public DomineDoor get_domine_door() => domine_door;

    public void end_credits_state(){
        domine_door.closed();
        StartCoroutine(end_credits_coroutine());
    }

    IEnumerator end_credits_coroutine(){
        yield return new WaitForSeconds(2);
        credits.Play("CreditsLoop");
        yield break;
    }
}
