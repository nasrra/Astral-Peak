using UnityEngine;

public class PlayerAnimator : MonoBehaviour{
    [SerializeField] public Animator a;
    private int state;
    public bool[] locked_layers = new bool[3];
    public readonly int 
        HEAD    = 0,
        BODY    = 1,
        LEGS    = 2,
        IDLE    = Animator.StringToHash("idle"),
        ATTACK  = Animator.StringToHash("attack"),
        RUN     = Animator.StringToHash("run"),
        GUARD   = Animator.StringToHash("guard");

    void Start() => state = IDLE;

    public void play(int animation_hash){
        play_head(animation_hash);
        play_body(animation_hash);
        play_legs(animation_hash);
        state = animation_hash;
    }

    public void play_head(int animation_hash){
        if(locked_layers[HEAD] == false){ a.Play(animation_hash, HEAD);}
    }

    public void play_body(int animation_hash){
        if(locked_layers[BODY] == false){ a.Play(animation_hash, BODY);}
    }

    public void play_legs(int animation_hash){
        if(locked_layers[LEGS] == false){ a.Play(animation_hash, LEGS);}
    }


    // used for animator to return to the current animation state.
    // layers that finish their animation, such as an attack, would return back to an idle or run animation.
    public void return_state(int layerIndex) => a.Play(state,layerIndex);

    // used for animator to lock a layer from changing their animation.
    public void lock_layer(int layerIndex) => locked_layers[layerIndex] = true; 
    public void unlock_layer(int layerIndex) => locked_layers[layerIndex] = false; 
}