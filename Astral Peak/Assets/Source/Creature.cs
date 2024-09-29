using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Creature : MonoBehaviour{
    [Header("Creature")]
    [SerializeField] protected Health health;
    [SerializeField] protected Animator animator;
    protected bool flippable = true;

    void Start(){
        link_events();
    }
    public Health get_health(){
        return health;
    }
    void OnDestroy(){
        unlink_events();
    }

    // flip the sprite.
    protected void flip_left() => transform.rotation = flippable? Quaternion.Euler(0, 180, 0) : transform.rotation;
    protected void flip_right() => transform.rotation = flippable? Quaternion.Euler(0, 0, 0) : transform.rotation;

    public void can_flip(int x) => flippable = x != 0;

    public void kill(){
        Debug.Log(gameObject.name + " has died!");
        Destroy(gameObject);
    }

    protected virtual void link_events(){
        Debug.Log("s");
        health.on_death += kill;
    }

    protected virtual void unlink_events(){
        health.on_death -= kill;
    }
}
