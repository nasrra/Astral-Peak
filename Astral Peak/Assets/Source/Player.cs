using System;
using System.Collections;
using Entropek;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : CreatureInheritor<CharacterMovement>{





    // Data
    public event Action
        damaged_start, damaged_stop, on_destroy;
    // static fields for other classes to access.
    public static Player instance;
    public static string spawn_point = "Enter", respawn_point = ""; // respawn is temporary but spawn is forever.
    [Header("Player")]
    [SerializeField] private PlayerAnimator animator;
    [SerializeField] protected MeleeHolsterHandler melee;
    [SerializeField] protected ParticleHandler particles;
    [SerializeField] protected PlayerSpriteHandler sprite;
    [SerializeField] protected AudioPlayer sound;
    [SerializeField] protected Collider2D col;
    private float invulnerable_time = 2;
    [SerializeField] bool up_toggle = false;


    // Base.
    void Awake(){
        instance = null;
        instance = this;
        sound.set_functions(new PlayerSound(sound));
        GameManager.link_player();
        load_data();
    }
    void Start(){   
        link_events();
        set_enter_position();
    }
    void OnDestroy(){
        GameManager.unlink_player();
        set_respawn_point(""); // reset respawn point;
        unlink_events();
        on_destroy?.Invoke();
    }
    void OnCollisionEnter2D(Collision2D other){  
        int layer = other.gameObject.layer;
        if(layer == LayersManager.ENEMY || layer == LayersManager.BOSS)
            handle_enemy_contact(other);
    }
    public void face_move_dir() => face_direction(movement.get_move_direction());




    // Movement.
    private void start_jump(){
        movement.set_jumping(true);
        movement.jump();
    }
    private void stop_jump(){
        movement.set_jumping(false);
        movement.end_jump();
    } 
    private void start_left()   => movement.move_left(true);
    private void start_right()  => movement.move_right(true);
    private void stop_left()    => movement.move_left(false);
    private void stop_right()   => movement.move_right(false);
    private void start_up()     => up_toggle = true;
    private void stop_up()      => up_toggle = false;
    private void attack(){
        if(up_toggle == true)
            animator.up_attack();
        else
            animator.side_attack();
    }
    private void dash(){
        // if we are in our invulnerable state no dashing.
        Vector2 move_direction = movement.get_move_direction();
        movement.dash(
            (move_direction.x == 0)?
            (flipped==false? Vector2.right : Vector2.left) :
                (move_direction.x == 1?     Vector2.right : Vector2.left) ,
            20, 
            0.25f); 
    }
    private void dashed(){
        sound.play_sound("dash");
        particles.emit_particle("dash");
        health.is_invulnerable();//
    }
    private void dash_end(){
        if(movement.check_grounded() == true)
            grounded();
        health.is_vulnerable();
    }
    private void grounded(){
        // bounce when hitting the ground.
        animator.medium_bounce();
        particles.play_ground_effected_particle("jump");
        sound.play_ground_effected_sound("jump");

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
    private void movement_animation(Vector2 direction){
        if(direction.x > 0 || direction.x < 0)
            animator.run(); // play run animation
        else
            animator.idle(); // play idle animation
    }
    private void jumped(){
        animator.jump();
        particles.play_ground_effected_particle("jump");
        sound.play_ground_effected_sound("jump");
    }
    private void new_ground(GameObject ground){
        sound.set_ground(ground.tag);
        particles.set_ground(ground.tag);
        transform.parent = ground.transform;
    }
    private void not_grounded(){
        animator.start_fall();
        transform.parent = null;
    }




    // Damaged and Health
    private void invulnerable() => col.excludeLayers = LayersManager.BITWISE_ENEMY | LayersManager.BITWISE_PROJECTILE;
    private void vulnerable(){
        // check if we have dashed into an enemy.
        Collider2D other = Physics2D.OverlapCircle(transform.position, 1, LayersManager.BITWISE_BOSS | LayersManager.BITWISE_ENEMY);
        if(other != null)
            handle_enemy_contact(other);
        col.excludeLayers = new LayerMask();
    }
    private void handle_enemy_contact(Collision2D other) => health.damage(new DamageData(1), new KnockbackData(10, 0.3f, other.transform));
    private void handle_enemy_contact(Collider2D other) => health.damage(new DamageData(1), new KnockbackData(10, 0.3f, other.transform));
    public override void kill() => StartCoroutine(death_state());
    private void damaged() => StartCoroutine(damaged_state());
    IEnumerator damaged_state(){
        sprite.play_damaged_flash();
        health.is_invulnerable(invulnerable_time);
        AudioManager.low_pass_audio(true);
        CameraController.instance.shake_camera(0.25f, 1, lock_shake: false);
        sound.play_sound("damaged");
        damaged_start?.Invoke();
        yield return new WaitForSeconds(invulnerable_time);
        damaged_stop?.Invoke();
        AudioManager.low_pass_audio(false);
    }
    IEnumerator death_state(){
        unlink_movement();
        unlink_input();
        unlink_melee();
        unlink_health();
        invulnerable();
        //AudioManager.low_pass_audio(true);
        movement.zero_velocity();
        CameraController.instance.shake_camera(0.25f, 1, lock_shake: false);
        sound.play_sound("damaged");
        sprite.play_death_effect(3f);
        animator.death();
        invoke_death_started();
        yield return new WaitForSeconds(3);
        //AudioManager.low_pass_audio(false);
        invoke_death_completed();
    }





    // Spawn & Door.
    public void set_spawn_point(string _spawn_point) => spawn_point = _spawn_point;
    public void set_respawn_point(string _respawn_point) => respawn_point = _respawn_point;
    public string get_spawn_point() => spawn_point;
    // used to set the players initial position in the scene.
    public void set_enter_position(){
        SpawnPoint spawn = SpawnPointManager.get_point(respawn_point != ""? respawn_point : spawn_point);
        if(spawn != null){
            transform.position = spawn.transform.position;
            if(spawn.get_movement() != MovementOption.NONE)
                StartCoroutine(door_exit_state(spawn));
        }
    }//
    public void door_enter_state(){
        InputManager.disable_user_input();
        transform.parent = null;
    }
    IEnumerator door_exit_state(SpawnPoint spawn){
        movement.clear_move_direction();//
        spawn.use_spawn();
        movement.movement(spawn.get_movement(), true);
        AudioManager.restore_sfx_smooth();  
        InputManager.disable_user_input();
        yield return new WaitForSeconds(1);
        movement.movement(spawn.get_movement(), false);
        InputManager.enable_user_input();
        yield break;
    }






    // Melee
    void attack_hit(){
        sound.play_sound("melee_hit");  
    } 





    // Game States.
    public override void enter_cutscene_state(){
        unlink_movement();
        animator.force_idle();
    }
    public override void exit_cutscene_state(){
        movement.renew();
        link_movement();
        movement.move_only_state();
    }
    protected override void entered_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            enter_cutscene_state();
    }
    protected override void exited_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            exit_cutscene_state();
    }





    // data serialissation:
    void load_data(){
        if(GameManager.is_data_loaded()==false)
            return;
        GameData data = GameManager.get_game_data();
        spawn_point = data.spawn_point;
    }
    void set_game_data(){
        if(GameManager.get_state() == GameState.CUTSCENE)
            return;
        GameData data = GameManager.get_game_data();
        data.spawn_point = spawn_point;
    }






    // Linkage
    private void unloaded_scene(Scene s) => unlink_events(); 
    protected void link_events(){
        link_input();
        link_melee();
        link_movement();
        link_health();
        link_game_manager();
        link_scene_manager();
        link_application();
    }
    protected void unlink_events(){
        unlink_movement();
        unlink_input();
        unlink_melee();
        unlink_health();
        unlink_game_manager();
        unlink_scene_manager();
        unlink_application();   
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
        InputManager.up_performed          += start_up;
        InputManager.up_cancelled          += stop_up;
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
        InputManager.up_performed          -= start_up;
        InputManager.up_cancelled          -= stop_up;
        InputManager.reset_input_blockers();
    }
    protected void link_movement(){
        CharacterMovement movement = get_movement() as CharacterMovement;
        movement.move_direction_changed += face_direction; 
        movement.move_direction_changed += movement_animation;
        movement.now_grounded           += grounded;
        movement.not_grounded           += not_grounded;
        movement.jumped                 += jumped;
        movement.dashed                 += dashed;
        movement.dash_end               += dash_end;
        movement.new_ground             += new_ground;
    }
    protected void unlink_movement(){
        CharacterMovement movement = get_movement() as CharacterMovement;
        movement.move_direction_changed -= face_direction; 
        movement.move_direction_changed -= movement_animation;
        movement.now_grounded           -= grounded;
        movement.not_grounded           -= not_grounded;
        movement.jumped                 -= jumped;
        movement.dashed                 -= dashed;
        movement.dash_end               -= dash_end;
        movement.new_ground             -= new_ground;
    }
    private void link_melee(){
        flipped_left            += particles.flip_particles_left;
        flipped_right           += particles.flip_particles_right;
        melee.hit               += attack_hit;
    }
    private void unlink_melee(){
        flipped_left            -= particles.flip_particles_left;
        flipped_right           -= particles.flip_particles_right;
        melee.hit               -= attack_hit;
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
    protected void link_scene_manager(){
        SceneManager.sceneUnloaded += unloaded_scene;
        CustomSceneManager.loading_scene += unlink_events;
    }
    protected void unlink_scene_manager(){
        SceneManager.sceneUnloaded -= unloaded_scene;
        CustomSceneManager.loading_scene -= unlink_events;
    }
    protected void link_application(){
        Application.quitting += unlink_events;    
    }
    protected void unlink_application(){
        Application.quitting -= unlink_events;
    }
    protected override void link_game_manager(){
        GameManager.set_game_data += set_game_data;
        base.link_game_manager();
    }
    protected override void unlink_game_manager(){
        GameManager.set_game_data -= set_game_data;
        base.unlink_game_manager();
    }
}
