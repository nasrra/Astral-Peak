using System;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : CreatureInheritor<CharacterMovement>{
    
    public event Action
        damaged_start, damaged_stop;

    // static fields for other classes to access.
    public static Player player;
    public static string exit_point = "";
    

    // data to link together.
    [Header("Player")]
    [SerializeField] private PlayerAnimator animator;
    [SerializeField] private Interactor interactor;
    [SerializeField] protected MeleeHolster melee;
    [SerializeField] protected PlayerParticlesHandler particles;
    [SerializeField] protected PlayerSpriteHandler sprite;
    [SerializeField] new protected PlayerAudio audio;
    [SerializeField] protected Collider2D col;
    private float invulnerable_time = 2;
    bool input_blocker = false; // used to avoid bug.

    void Awake(){
        player = this;
        GameManager.link_player();
    }

    void Start(){   
        link_events();
        set_enter_position();
        // snap camera to players new position.
        CameraController.instance.snap_to_target(); 
    }

    void OnDestroy(){
        GameManager.unlink_player();
        unlink_events();
    }

    void OnCollisionEnter2D(Collision2D other){  
        if(other.gameObject.layer == LayersManager.ENEMY)
            handle_enemy_contact(other);
    }

    // used to avoid bug where unlinking and relinking movement:
    // holding down left or right will break move direction and cause player to only go in that one direction.

    // known bug: when holding down both keys then letting go one at a time after the blocker is true, will still cause the problem.
    private void start_movement(Action movement){
        movement();
        input_blocker = false;
    }
    private void stop_movement(Action movement){
        if(input_blocker == false)
            movement();
    }

    private void start_jump()   => start_movement(()=>movement.jump());
    private void start_left()   => start_movement(()=>movement.move_left(true));
    private void start_right()  => start_movement(()=>movement.move_right(true));
    private void stop_jump()    => stop_movement(()=>movement.end_jump());
    private void stop_left()    => stop_movement(()=>movement.move_left(false));
    private void stop_right()   => stop_movement(()=>movement.move_right(false));
    private void interact()     => interactor.interact();
    private void attack()       => animator.Play(PlayerAnimator.ATTACK);
    private void dash(){
        // if we are in our invulnerable state no dashing.
        if(health.invulnerable == true)
            return;
        
        float move_dir = movement.get_move_direction().x;
        if(move_dir > 0)
            movement.dash(Vector2.right, 20, 0.25f);
        else 
            movement.dash(Vector2.left, 20, 0.25f);

    }

    private void dashed(){
        audio.emit_dash();
        particles.emit_dash();
        health.is_invulnerable();
    }

    private void grounded(){
        // bounce when hitting the ground.
        animator.medium_bounce();

        // reset to none so that the animator can play the run or idle animation.
        animator.none();
        
        // if we are moving play the run animation, if not, play the idle one.
        float move_dir = movement.get_move_direction().x;
        if(move_dir > 0 || move_dir < 0){
            animator.run();
        }
        else{
            animator.idle();
        }
    }

    private void invulnerable(){
        col.excludeLayers = LayersManager.BITWISE_ENEMY | LayersManager.BITWISE_PROJECTILE;
    }
    private void vulnerable(){
        col.excludeLayers = new LayerMask();
    }

    private void damaged() => StartCoroutine(damaged_state());
    IEnumerator damaged_state(){
        sprite.play_damaged_flash();
        health.is_invulnerable(invulnerable_time);
        damaged_start?.Invoke();
        yield return new WaitForSeconds(invulnerable_time);
        damaged_stop?.Invoke();
    }

    // used to set the players initial position in the scene.
    public void set_enter_position(){
        if(exit_point != "")
            transform.position = DoorManager.get_position(exit_point);
    }

    private void handle_enemy_contact(Collision2D other) => health.damage(new DamageData(1), new KnockbackData(10, 0.3f, other.transform));

    private void movement_animation(){
        Vector2 direction = get_movement().get_move_direction();
        if(direction.x > 0 || direction.x < 0)
            animator.run(); // play run animation
        else
            animator.idle(); // play idle animation
    }

    public override void enter_cutscene_state(){
        unlink_input();
        unlink_movement();
        movement.stop();
        animator.idle();
    }

    public override void exit_cutscene_state(){
        //movement.stop();
        link_input();
        link_movement();
    }

    protected void attack_failed(Collider2D other) => movement.knockback(new KnockbackData(melee.get_self_knockback_force(), melee.get_self_knockback_duration(), other.transform));

    protected void link_events(){
        link_input();
        link_melee();
        link_movement();
        link_health();
    }

    protected void unlink_events(){
        unlink_input();
        unlink_melee();
        unlink_movement();
        link_health();
    }

    public void link_input(){
        InputManager.jump_performed        += start_jump;
        InputManager.jump_cancelled        += stop_jump;
        InputManager.left_performed        += start_left;
        InputManager.left_cancelled        += stop_left;
        InputManager.right_performed       += start_right;
        InputManager.right_cancelled       += stop_right;
        InputManager.interact_performed    += interact;
        InputManager.attack_performed      += attack;
        InputManager.dash_performed        += dash; 
        input_blocker = true; 
    }

    public void unlink_input(){
        InputManager.jump_performed        -= start_jump;
        InputManager.jump_cancelled        -= stop_jump;
        InputManager.left_performed        -= start_left;
        InputManager.left_cancelled        -= stop_left;
        InputManager.right_performed       -= start_right;
        InputManager.right_cancelled       -= stop_right;
        InputManager.interact_performed    -= interact;
        InputManager.attack_performed      -= attack;
        InputManager.dash_performed        -= dash;    
        input_blocker = false;
    }

    protected void link_movement(){
        CharacterMovement movement = get_movement() as CharacterMovement;
        movement.move_direction_changed += face_move_dir; 
        movement.move_direction_changed += movement_animation;
        movement.now_grounded           += grounded;
        movement.not_grounded           += animator.start_fall;
        movement.jumped                 += animator.jump;
        movement.dashed                 += dashed;
        movement.dash_end               += health.is_vulnerable;
    }
    protected void unlink_movement(){
        CharacterMovement movement = get_movement() as CharacterMovement;
        movement.move_direction_changed -= face_move_dir;
        movement.move_direction_changed -= movement_animation;
        movement.now_grounded           -= grounded;
        movement.not_grounded           -= animator.start_fall;
        movement.jumped                 -= animator.jump;
        movement.dashed                 -= dashed;
        movement.dash_end               -= health.is_vulnerable;
    }

    private void link_melee(){
        melee.hit_enemy_guard   += attack_failed;
        flipped_left            += particles.flip_left;
        flipped_right           += particles.flip_right;
    }
    private void unlink_melee(){
        melee.hit_enemy_guard   -= attack_failed;
        flipped_left            -= particles.flip_left;
        flipped_right           -= particles.flip_right;
    }

    protected void link_health(){
        health.now_invulnerable += invulnerable;
        health.now_vulnerable   += vulnerable;
        health.damaged          += damaged;
        health.knockback        += get_movement().knockback;
        health.death            += kill;
        health.death            += get_movement().StopAllCoroutines;    
    }

    protected void unlink_health(){
        health.now_invulnerable -= invulnerable;
        health.now_vulnerable   -= vulnerable;
        health.damaged          -= damaged;
        health.knockback        -= get_movement().knockback;
        health.death            -= kill;
        health.death            -= get_movement().StopAllCoroutines;    
    }
}
