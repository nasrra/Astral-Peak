using System.Linq.Expressions;
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
    [SerializeField] protected PlayerParticlesHandler particles;
    [SerializeField] protected PlayerAnimator animator;
    [SerializeField] protected Collider2D col;

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
    private void attack()       => animator.attack();
    private void dash(){
        float move_dir = movement.get_move_direction().x;
        if(move_dir > 0)
            movement.dash(Vector2.right, 20, 0.25f);
        else 
            movement.dash(Vector2.left, 20, 0.25f);
    }

    private void grounded(){
        // bounce when hitting the ground.
        animator.medium_bounce();

        // reset to none so that the animator can play the run or idle animation.
        animator.none();
        
        // if we are moving play the run animation, if not, play the idle one.
        float move_dir = movement.get_move_direction().x;
        if(move_dir > 0 || move_dir < 0)
            animator.run();
        else
            animator.idle();
    }

    private void invulnerable(){
        col.excludeLayers = LayersManager.BITWISE_ENEMY | LayersManager.BITWISE_PROJECTILE;
    }
    private void vulnerable(){
        col.excludeLayers = new LayerMask();
    }

    // used to set the players initial position in the scene.
    public void set_enter_position(){
        if(exit_point != "")
            transform.position = DoorManager.instance.get_position(exit_point);
    }

    private void handle_enemy_contact(Collision2D other){
        // knockback the player.    
        movement.knockback(transform.position - other.transform.position, 10, 0.3f);
        // damage the player.
        damage(1);
    }

    protected override void guarded_attack(){
        base.guarded_attack();
        animator.idle();
    }

    protected override void parried_attack(){
        base.guarded_attack();
        animator.idle();
    }

    private void attack_failed(Collider2D other){
        movement.knockback(
            transform.position - other.transform.position, 
            melee.get_self_knockback_force(), 
            melee.get_self_knockback_duration()
        );
    }

    private void movement_animation(){
        Vector2 direction = get_movement().get_move_direction();
        if(direction.x > 0 || direction.x < 0)
            animator.run(); // play run animation
        else
            animator.idle(); // play idle animation
    }

    #region Linkage
    protected override void link_events(){
        base.link_events();
        link_input();
        link_melee();
        link_movement();
    }

    protected override void unlink_events(){
        base.unlink_events();
        unlink_input();
        unlink_melee();
        unlink_movement();
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
        input.dash_performed        += dash;  
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
        input.dash_performed        -= dash;    
    }

    protected void link_movement(){
        CharacterMovement movement = get_movement() as CharacterMovement;
        movement.move_direction_changed += movement_animation;
        movement.now_grounded           += grounded;
        movement.not_grounded           += animator.start_fall;
        movement.jumped                 += animator.jump;
        movement.dashed                 += health.is_invulnerable;
        movement.finish_dash            += health.is_vulnerable;
    }
    protected void unlink_movement(){
        CharacterMovement movement = get_movement() as CharacterMovement;
        movement.move_direction_changed -= movement_animation;
        movement.now_grounded           -= grounded;
        movement.not_grounded           -= animator.start_fall;
        movement.jumped                 -= animator.jump;
        movement.dashed                 -= health.is_invulnerable;
        movement.finish_dash            -= health.is_vulnerable;
    }

    private void link_melee(){
        melee.hit_enemy_guard += attack_failed;
        flipped_left += particles.flip_left;
        flipped_right += particles.flip_right;
    }
    private void unlink_melee(){
        melee.hit_enemy_guard -= attack_failed;
        flipped_left -= particles.flip_left;
        flipped_right -= particles.flip_right;
    }

    protected override void link_health(){
        base.link_health();
        health.now_invulnerable += invulnerable;
        health.now_vulnerable += vulnerable;
    }

    protected override void unlink_health(){
        base.unlink_health();
        health.now_invulnerable -= invulnerable;
        health.now_vulnerable -= vulnerable;
    }

    #endregion
}
