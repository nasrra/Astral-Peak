using UnityEngine;
using UnityEngine.InputSystem;

public class Player : CreatureInheritor<CharacterMovement>{
    
    // static fields for other classes to access.
    public static Player player;
    public static string exit_point = "";
    

    // data to link together.
    [Header("Player")]
    [SerializeField] private InputManager input;
    [SerializeField] private Interactor interactor;
    [SerializeField] protected MeleeHolster melee;
    [SerializeField] protected PlayerAnimator a;

    void Awake(){
        player = this;
    }

    void Start(){   
        link_events();
        set_enter_position();
        // snap camera to players new position.
        CameraController.instance.snap_to_target(); 
    }

    void OnDestroy(){
        unlink_events();
    }

    void OnCollisionEnter2D(Collision2D other){  
        if(other.gameObject.tag == "Enemy")
            handle_enemy_contact(other);
    }

    private void start_jump()   => movement.jump();
    private void stop_jump()    => movement.end_jump();
    private void start_left()   => movement.move_left(true);
    private void stop_left()    => movement.move_left(false);
    private void start_right()  => movement.move_right(true);
    private void stop_right()   => movement.move_right(false);
    private void interact()     => interactor.interact();
    private void attack()       => a.attack();
    private void parry()        => a.guard();

    // used to set the players initial position in the scene.
    public void set_enter_position(){
        if(exit_point != "")
            transform.position = DoorManager.instance.get_position(exit_point);
    }

    private void handle_enemy_contact(Collision2D other){
        // knockback the player.    
        movement.knockback(transform.position - other.transform.position, 10, 0.3f);
        // damage the player.
        damage(2, gameObject);
    }

    protected override void guarded_attack(){
        base.guarded_attack();
        a.idle();
    }

    private void attack_failed(Collider2D other){
        movement.knockback(
            transform.position - other.transform.position, 
            melee.get_self_knockback_force(), 
            melee.get_self_knockback_duration()
        );
        damage(
            melee.get_self_damage(), 
            other.gameObject
        );
    }

    #region Linkage
    protected override void link_events(){
        base.link_events();
        link_input();
        link_melee();
    }

    protected override void unlink_events(){
        base.unlink_events();
        unlink_input();
        unlink_melee();
    }

    private void link_input(){
        input.jump_performed        += start_jump;
        input.jump_cancelled        += stop_jump;
        input.left_performed        += start_left;
        input.left_cancelled        += stop_left;
        input.right_performed       += start_right;
        input.right_cancelled       += stop_right;
        input.interact_performed    += interact;
        input.attack_performed      += attack;
        input.parry_performed       += parry;  
    }

    private void unlink_input(){
        input.jump_performed        -= start_jump;
        input.jump_cancelled        -= stop_jump;
        input.left_performed        -= start_left;
        input.left_cancelled        -= stop_left;
        input.right_performed       -= start_right;
        input.right_cancelled       -= stop_right;
        input.interact_performed    -= interact;
        input.attack_performed      -= attack;
        input.parry_performed       -= parry;        
    }

    private void link_melee(){
        melee.hit_enemy_guard += attack_failed;
    }

    private void unlink_melee(){
        melee.hit_enemy_guard -= attack_failed;
    }

#endregion
}
