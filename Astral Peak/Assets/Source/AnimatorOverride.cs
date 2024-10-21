using Unity.VisualScripting;
using UnityEngine;

public class AnimatorOverride : MonoBehaviour{
    [SerializeField] protected Animator a;
    protected int state;
    public bool[] locked_layers = new bool[3];
    public readonly int
        HEAD        = 0,
        BODY        = 1,
        LEGS        = 2,
        OVERRIDE    = 3;

    protected void play_head(int animation_hash){ if(locked_layers[HEAD] == false && a.HasState(HEAD, animation_hash) == true) a.Play(animation_hash, HEAD);}
    protected void play_body(int animation_hash){ if(locked_layers[BODY] == false && a.HasState(BODY, animation_hash) == true) a.Play(animation_hash, BODY);}
    protected void play_legs(int animation_hash){ if(locked_layers[LEGS] == false && a.HasState(LEGS, animation_hash) == true) a.Play(animation_hash, LEGS);}
    protected void play_override(int animation_hash){a.Play(animation_hash, OVERRIDE);}

    public void play(int animation_hash, bool set_state){
        play_head(animation_hash);
        play_body(animation_hash);
        play_legs(animation_hash);
        state = set_state == true? animation_hash : state;
    }

    // used for animator to return to the current animation state.
    // layers that finish their animation, such as an attack, would return back to an idle or run animation.
    public void return_state(int layerIndex) => a.Play(state,layerIndex);
    public void return_state() => play(state, false);

    // used for animator to lock a layer from changing their animation.
    public void lock_layer(int layerIndex) => locked_layers[layerIndex] = true; 
    public void unlock_layer(int layerIndex) => locked_layers[layerIndex] = false; 

    public Animator get_animator() => a;
    public int get_state() => state;
}