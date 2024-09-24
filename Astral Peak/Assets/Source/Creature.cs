using UnityEngine;

public class Creature : MonoBehaviour{
    [Header("Creature")]
    [SerializeField] protected Health health;
    [SerializeField] protected Animator animator;
    void Start(){
        link_events();
    }
    public Health get_health(){
        return health;
    }

    // flip the sprite.
    protected void flip_left(){
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    protected void flip_right(){
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void Kill(){
        Debug.Log(gameObject.name + " has died!");
        Destroy(gameObject);
    }

    void OnDestroy(){
        unlink_events();
    }

    private void link_events(){
        health.on_death += Kill;
    }

    private void unlink_events(){
        health.on_death -= Kill;
    }
}
