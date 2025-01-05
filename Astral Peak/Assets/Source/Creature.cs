using System;
using System.Collections;
using Deluz;
using UnityEngine;

/// <summary>
/// use this class for when inheriting from the creature class.
/// </summary>
/// <typeparam name="T">Movemnt Type</typeparam>
public abstract class CreatureInheritor<T> : Creature where T : Movement{
    [SerializeField] protected T movement;
    public override Movement get_movement() => movement;
}

/// <summary>
/// This class is used only for when the Creature class needs to be accessed externally by other objects.
/// It is also the class that defines all functionality for the inheritor class.
/// </summary>
public abstract class Creature : MonoBehaviour{
    public event Action flipped_right, flipped_left, death;

    [Header("Creature")]
    [SerializeField] protected Health health;
    protected bool flippable = true;

    protected virtual void state_switch(ref Coroutine state, IEnumerator _state){
        if(state != null)
            StopCoroutine(state);
        state = StartCoroutine(_state);
    }

    public Health get_health() => health;
    // the inheritor class returns which movement it is using.
    public abstract Movement get_movement();

    // flip the creature in relation to where they are moving towards.
    public void face_direction(Vector2 direction){
        if(direction.x < 0)
            flip_left();
        else if(direction.x > 0)
            flip_right();
    }

    // flip the sprite.
    protected void flip_left(){
        if(flippable == true){
            transform.rotation = Quaternion.Euler(0, 180, 0);
            flipped_left?.Invoke();
        }
    }
    protected void flip_right(){
        if(flippable == true){
            transform.rotation = Quaternion.Euler(0, 0, 0);
            flipped_right?.Invoke();
        }
    }
    protected void flip(){
        if(flippable == true){
            if(transform.rotation.y == 0)
                flip_left();
            else
                flip_right();
        }
    }

    public void can_flip(int x) => flippable = x != 0;

    /// <summary>
/// this should only be used on Awake(), Start(), OnEnable(), to check what the game state is that the creature has initialized into.
    /// </summary>
    protected void check_game_state() => entered_game_state(GameManager.get_state());
    public virtual void enter_cutscene_state(){Log.MethodNotImplemented(this);}
    public virtual void exit_cutscene_state(){Log.MethodNotImplemented(this);}
    protected virtual void link_game_manager(){
        GameManager.entered_game_state += entered_game_state;
        GameManager.exited_game_state  += exited_game_state;
    }
    protected virtual void unlink_game_manager(){
        GameManager.entered_game_state -= entered_game_state;
        GameManager.exited_game_state  -= exited_game_state;        
    }
    protected virtual void entered_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            enter_cutscene_state();
    }
    protected virtual void exited_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            exit_cutscene_state();
    }
    public virtual void kill() => death?.Invoke();
}
