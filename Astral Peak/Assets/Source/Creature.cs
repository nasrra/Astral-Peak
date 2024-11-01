using System;
using Unity.Mathematics;
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
    public event Action flipped_right, flipped_left;

    [Header("Creature")]
    [SerializeField] protected Health health;
    //[SerializeField] protected Guard guard;
    [SerializeField] protected bool stunnable;
    protected bool flippable = true;

    void Start(){
        link_events();
    }
    void OnDestroy(){
        unlink_events();
    } 

    public Health get_health() => health;
    // the inheritor class returns which movement it is using.
    public abstract Movement get_movement();

    // flip the creature in relation to where they are moving towards.
    public void face_move_dir(){
        Vector2 direction = get_movement().get_move_direction();
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

    public bool damage(float amt) => health.damage(amt);

    protected virtual void kill() => Destroy(gameObject);
    //public void is_guarding(int x) => guard.is_guarding(x);
    //public void is_parrying(int x) => guard.is_parrying(x);
    public void is_stunnable(int x) => stunnable = x != 0;
    protected virtual void parried_attack() => Debug.Log(gameObject.name + " has parried!");
    protected virtual void guarded_attack(){ Debug.Log(gameObject.name + " has guarded!");}

#region Linkage
    protected virtual void link_events(){
        link_health();
        get_movement().move_direction_changed += face_move_dir;
    }

    protected virtual void unlink_events(){
        unlink_health();
        get_movement().move_direction_changed -= face_move_dir;
    }

    private void link_health() => health.death += kill;
    private void unlink_health() => health.death -= kill;
#endregion
}
