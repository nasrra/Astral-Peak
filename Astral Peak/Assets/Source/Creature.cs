using System.Security.Cryptography;
using Unity.IO.LowLevel.Unsafe;
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
    [Header("Creature")]
    [SerializeField] protected Health health;
    [SerializeField] protected Animator animator;
    protected bool flippable = true;

    void Start() => link_events();
    void OnDestroy() => unlink_events();

    public Health get_health() => health;
    // the inheritor class returns which movement it is using.
    public abstract Movement get_movement();

    // flip the creature in relation to where they are moving towards.
    protected void face_move_dir(Vector2 direction){
        if(direction.x < 0)
            flip_left();
        else if(direction.x > 0)
            flip_right();
    }

    // flip the sprite.
    protected void flip_left() => transform.rotation = flippable? Quaternion.Euler(0, 180, 0) : transform.rotation;
    protected void flip_right() => transform.rotation = flippable? Quaternion.Euler(0, 0, 0) : transform.rotation;

    public void can_flip(int x) => flippable = x != 0;

    public void kill(){
        Debug.Log(gameObject.name + " has died!");
        Destroy(gameObject);
    }

    private void on_death(GameObject other) => kill();

    protected virtual void link_events(){
        health.on_death += on_death;
        get_movement().move_direction_changed += face_move_dir;
    }

    protected virtual void unlink_events(){
        health.on_death -= on_death;
        get_movement().move_direction_changed -= face_move_dir;
    }
}
