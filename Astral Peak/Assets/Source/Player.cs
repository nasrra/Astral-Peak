using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : CreatureInheritor<CharacterMovement>{
    
    public event Action
        damaged_start, damaged_stop, death_start;

    // static fields for other classes to access.
    public static Player instance;
    public static string spawn_point = "", respawn_point = ""; // respawn is temporary but spawn is forever.


    // data to link together.
    [Header("Player")]
    [SerializeField] private PlayerAnimator animator;
    [SerializeField] protected PlayerCombat melee;
    [SerializeField] protected PlayerParticlesHandler particles;
    [SerializeField] protected PlayerSpriteHandler sprite;
    [SerializeField] new protected PlayerAudio audio;
    [SerializeField] protected Collider2D col;
    private float invulnerable_time = 2;

    void Awake(){
        instance = this;
        Application.quitting += unlink_events;
        SceneManager.sceneUnloaded += unloaded;
        GameManager.link_player();
    }

    void unloaded(Scene s) => unlink_events(); 

    void Start(){   
        link_events();
        set_enter_position();
        // snap camera to players new position.
        CameraController.instance.snap_to_target(); 
    }

    void OnDestroy(){
        GameManager.unlink_player();
        set_respawn_point(""); // reset respawn point;
        unlink_events();
        Application.quitting -= unlink_events;
    }

    void OnCollisionEnter2D(Collision2D other){  
        if(other.gameObject.layer == LayersManager.ENEMY)
            handle_enemy_contact(other);
    }

    // used to avoid bug where unlinking and relinking movement:
    // holding down left or right will break move direction and cause player to only go in that one direction.

    private void start_jump(){
        movement.set_jumping(true);
        movement.jump();
        //movement.coyote_jump_timer();
    }
    private void stop_jump(){
        movement.set_jumping(false);
        movement.end_jump();
    } 
    private void start_left()   => movement.move_left(true);
    private void start_right()  => movement.move_right(true);
    private void stop_left()    => movement.move_left(false);
    private void stop_right()   => movement.move_right(false);
    private void attack()       => animator.Play(PlayerAnimator.ATTACK);
    private void dash(){
        // if we are in our invulnerable state no dashing.
        Vector2 move_direction = movement.get_move_direction();
        movement.dash(
            (move_direction.magnitude == 0)?
                (transform.rotation.y == 0? Vector2.right : Vector2.left) :
                (move_direction.x == 1?     Vector2.right : Vector2.left) ,
            20, 
            0.25f); 
    }

    private void dashed(){
        audio.emit_dash();
        particles.emit_dash();
        health.is_invulnerable();//
    }

    private void grounded(){
        // bounce when hitting the ground.
        animator.medium_bounce();
        particles.emit_jump();
        audio.emit_grounded();

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
        AudioManager.low_pass_audio(true);
        CameraController.instance.shake_camera(0.25f, 1);
        audio.emit_damaged();
        damaged_start?.Invoke();
        yield return new WaitForSeconds(invulnerable_time);
        damaged_stop?.Invoke();
        AudioManager.low_pass_audio(false);
    }

    public void set_spawn_point(string _spawn_point) => spawn_point = _spawn_point;
    public void set_respawn_point(string _respawn_point) => respawn_point = _respawn_point;
    public string get_spawn_point() => spawn_point;

    // used to set the players initial position in the scene.
    public void set_enter_position(){
        SpawnPoint spawn = SpawnPointManager.get_point(respawn_point != ""? respawn_point : spawn_point);
        if(spawn != null){
            if(spawn.get_movement() != MovementOption.NONE)
                StartCoroutine(door_exit_state());
            transform.position = spawn.transform.position;
        }
    }

    private void handle_enemy_contact(Collision2D other) => health.damage(new DamageData(1), new KnockbackData(10, 0.3f, other.transform));

    private void movement_animation(){
        Vector2 direction = get_movement().get_move_direction();
        if(direction.x > 0 || direction.x < 0)
            animator.run(); // play run animation
        else
            animator.idle(); // play idle animation
    }


    public void door_enter_state(){
        unlink_input();
    }

    IEnumerator door_exit_state(){
        unlink_input();
        SpawnPoint spawn = SpawnPointManager.get_point(spawn_point);
        transform.position = spawn.transform.position;
        movement.movement(spawn.get_movement(), true);
        AudioManager.restore_sfx_smooth();  
        yield return new WaitForSeconds(1);
        movement.movement(spawn.get_movement(), false);
        link_input();
        yield break;
    }

    public override void enter_cutscene_state(){
        unlink_input();
        unlink_movement();
        animator.cutscene_idle();
        movement.stop();
    }

    public override void exit_cutscene_state(){
        link_input();
        link_movement();
    }

    public override void kill() => StartCoroutine(death_state());

    IEnumerator death_state(){
        unlink_events();
        invulnerable();
        //AudioManager.low_pass_audio(true);
        CameraController.instance.shake_camera(0.25f, 1);
        audio.emit_damaged();
        sprite.play_death_effect(2);
        animator.death();
        death_start?.Invoke();
        yield return new WaitForSeconds(3);
        
        //AudioManager.low_pass_audio(false);
        base.kill();
    }

    protected void link_events(){
        link_input();
        link_melee();
        link_movement();
        link_health();
    }

    protected void unlink_events(){
        unlink_movement();
        unlink_input();
        unlink_melee();
        unlink_health();
    } 

    public void link_input(){
        InputManager.jump_performed        += start_jump;
        InputManager.jump_cancelled        += stop_jump;
        InputManager.left_performed        += start_left;
        InputManager.left_cancelled        += stop_left;
        InputManager.right_performed       += start_right;
        InputManager.right_cancelled       += stop_right;
        InputManager.attack_performed      += attack;
        InputManager.dash_performed        += dash; 
        InputManager.reset_input_blockers();
    }

    public void unlink_input(){
        InputManager.jump_performed        -= start_jump;
        InputManager.jump_cancelled        -= stop_jump;
        InputManager.left_performed        -= start_left;
        InputManager.left_cancelled        -= stop_left;
        InputManager.right_performed       -= start_right;
        InputManager.right_cancelled       -= stop_right;
        InputManager.attack_performed      -= attack;
        InputManager.dash_performed        -= dash;    
        InputManager.reset_input_blockers();
    }

    protected void link_movement(){
        CharacterMovement movement = get_movement() as CharacterMovement;
        movement.move_direction_changed += face_move_dir; 
        movement.move_direction_changed += movement_animation;
        movement.now_grounded           += grounded;
        movement.not_grounded           += animator.start_fall;
        movement.jumped                 += animator.jump;
        movement.jumped                 += particles.emit_jump;
        movement.jumped                 += audio.emit_jump;
        movement.dashed                 += dashed;
        movement.dash_end               += health.is_vulnerable;
        movement.new_ground             += audio.set_ground;
        movement.new_ground             += particles.set_ground;
        movement.stop();
    }
    protected void unlink_movement(){
        CharacterMovement movement = get_movement() as CharacterMovement;
        movement.move_direction_changed -= face_move_dir;
        movement.move_direction_changed -= movement_animation;
        movement.now_grounded           -= grounded;
        movement.not_grounded           -= animator.start_fall;
        movement.jumped                 -= animator.jump;
        movement.jumped                 -= particles.emit_jump;
        movement.jumped                 -= audio.emit_jump;
        movement.dashed                 -= dashed;
        movement.dash_end               -= health.is_vulnerable;
        movement.new_ground             -= audio.set_ground;
        movement.new_ground             -= particles.set_ground;
    }

    private void link_melee(){
        flipped_left            += particles.flip_left;
        flipped_right           += particles.flip_right;
        melee.melee_hit         += audio.emit_attack_hit;
    }
    private void unlink_melee(){
        flipped_left            -= particles.flip_left;
        flipped_right           -= particles.flip_right;
        melee.melee_hit         -= audio.emit_attack_hit;
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
