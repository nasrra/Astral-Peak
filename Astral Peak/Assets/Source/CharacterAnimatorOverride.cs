using UnityEngine;

public class CharacterAnimatorOverride : MonoBehaviour{
    [SerializeField] protected Animator animator;
    protected int state;
    public bool[] locked_layers = new bool[3];
    public readonly int
        HEAD                = 0,
        BODY                = 1,
        LEGS                = 2,
        BOUNCE_OVERRIDE     = 3,
        BODY_OVERRIDE       = 4;

    protected void play_head(int animation_hash){ if(locked_layers[HEAD] == false && animator.HasState(HEAD, animation_hash) == true) animator.CrossFade(animation_hash, 0.1f, HEAD,0);}
    protected void play_body(int animation_hash){ if(locked_layers[BODY] == false && animator.HasState(BODY, animation_hash) == true) animator.CrossFade(animation_hash, 0.1f, BODY,0);}
    protected void play_legs(int animation_hash){ if(locked_layers[LEGS] == false && animator.HasState(LEGS, animation_hash) == true) animator.CrossFade(animation_hash, 0.1f, LEGS,0);}
    protected void play_head_instant(int animation_hash){ 
        if(locked_layers[HEAD] == false && animator.HasState(HEAD, animation_hash) == true){
            animator.Play(animation_hash, HEAD,0);
            animator.Update(0);
        } 
    }
    protected void play_body_instant(int animation_hash){
        if(locked_layers[BODY] == false && animator.HasState(BODY, animation_hash) == true){
            animator.Play(animation_hash, BODY,0);
            animator.Update(0);
        } 
    }
    protected void play_legs_instant(int animation_hash){ 
        if(locked_layers[LEGS] == false && animator.HasState(LEGS, animation_hash) == true){
            animator.Play(animation_hash, LEGS,0);
            animator.Update(0);
        }
    }
    protected void play_bounce_override(int animation_hash){
        animator.Play(animation_hash, BOUNCE_OVERRIDE);
    }
    protected void play_body_override(int animation_hash){
        animator.CrossFade(animation_hash, 0.1f, BODY_OVERRIDE, 0);
    }


    public void play(int animation_hash, bool set_state){
        if( animator.isActiveAndEnabled == true){
            play_head(animation_hash);
            play_body(animation_hash);
            play_legs(animation_hash);
            state = set_state == true? animation_hash : state;
        }
    }

    public void play_instant(int animation_hash, bool set_state){
        if( animator.isActiveAndEnabled == true){
            play_head_instant(animation_hash);
            play_body_instant(animation_hash);
            play_legs_instant(animation_hash);
            state = set_state == true? animation_hash : state;
        }
    }

    // used for animator to return to the current animation state.
    // layers that finish their animation, such as an attack, would return back to an idle or run animation.
    public void return_state(int layerIndex) => animator.Play(state,layerIndex);
    public void return_state() => play(state, false);

    // used for animator to lock a layer from changing their animation.
    public void lock_layer(int layerIndex) => locked_layers[layerIndex] = true; 
    public void unlock_layer(int layerIndex) => locked_layers[layerIndex] = false; 

    public void unlock_layers(){
        for (int i = 0; i < locked_layers.Length; i++)
            locked_layers[i] = false;
    }

    public Animator get_animator() => animator;
    public int get_state() => state;
}