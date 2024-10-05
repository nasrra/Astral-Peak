using UnityEngine;

public class PlayerAnimator : MonoBehaviour{
    [SerializeField] public Animator a;
    readonly int 
        IDLE    = Animator.StringToHash("idle"),
        ATTACK  = Animator.StringToHash("attack"),
        GUARD   = Animator.StringToHash("guard");

    public void idle()      => a.Play(IDLE);
    public void attack()    => a.Play(ATTACK);
    public void guard()     => a.Play(GUARD);
}