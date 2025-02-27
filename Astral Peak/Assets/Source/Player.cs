using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : CreatureInheritor<CharacterMovement>{





    // Data
    public event Action
        damaged_start, damaged_stop, on_destroy, entered_door, exiting_door, exited_door, intermediate_health_updated, intermediate_health_gained;
    // static fields for other classes to access.
    public static Player instance;
    public static string spawn_point = "Enter", respawn_point = ""; // respawn is temporary but spawn is forever.
    private string ground;
    bool i_frames = false;
    [Header("Player")]
    [SerializeField] protected PlayerAnimator animator;
    [SerializeField] protected MeleeHolsterHandler melee;
    [SerializeField] protected ParticleHandler particles;
    [SerializeField] protected PlayerSpriteHandler sprite;
    [SerializeField] protected AudioPlayer sound;
    [SerializeField] protected Collider2D col;
    [SerializeField] protected StudioListener audio_listener;
    private HashSet<Action> door_movement_queue = new HashSet<Action>();
    private float invulnerable_time = 2;
    [SerializeField] float intermediate_health = 0;
    [SerializeField] bool up_toggle = false;



 

    // Base.
    void Awake(){
        instance = this;
        AudioManager.load_bank("entity_player");
        AudioManager.load_bank("entity_creature");
        GameManager.link_player();
        movement.move_only_state();
        load_data();
        link_events();
    }
    void Start(){   
        set_enter_position();
    }
    void OnDestroy(){
        AudioManager.unload_bank("entity_player");
        AudioManager.unload_bank("entity_creature");
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
    private void start_left()  {movement.move_left(true);    }
    private void start_right() {movement.move_right(true);   }
    private void stop_left()   {movement.move_left(false);   }
    private void stop_right()  {movement.move_right(false);  }
    private void start_up()    {enabled_toggle_up();         }
    private void stop_up()     {disabled_toggle_up();        }
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
            28, 
            0.25f); 
    }
    private void dashed(){
        sound.play_diegetic_one_shot("player_dash");
        particles.emit_particle("dash");
        health.set_guarded();//
    }
    private void dash_end(){
        if(movement.check_grounded() == true)
            grounded();
        if(i_frames == false)
            health.set_vulnerable();
    }
    private void grounded(){
        // bounce when hitting the ground.
        animator.medium_bounce();
        particles.play_ground_effected_particle("jump");
        sound.play_diegetic_one_shot($"{ground}_impact_light");

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
    private void movement_updated(Vector2 direction){
        if(direction.x > 0 || direction.x < 0){
            animator.run(); // play run animation
        }
        else{
            animator.idle(); // play idle animation
        }

    }
    private void jumped(){
        animator.jump();
        particles.play_ground_effected_particle("jump");
        sound.play_diegetic_one_shot($"{ground}_impact_light");
    }
    private void new_ground(GameObject ground){
        // sound.set_ground(ground.tag);
        particles.set_ground(ground.tag);
        this.ground = ground.tag;
        if(ground.layer == LayersManager.PLATFORM)
            transform.parent = ground.transform;
    }
    private void not_grounded(){
        animator.start_fall();
        transform.parent = null;
    }
    private void enabled_toggle_up(){
        up_toggle = true;
        animator.up_toggle();
    }
    private void disabled_toggle_up(){
        up_toggle = false;
        animator.stop_up_toggle();
    }
    public void play_footstep_sound(){
        if(string.IsNullOrEmpty(ground)!=true)
            sound.play_diegetic_one_shot($"{ground}_footstep");
    }


    // Damaged and Health
    private void invulnerable() => col.excludeLayers = LayersManager.BITWISE_ENEMY | LayersManager.BITWISE_PROJECTILE | LayersManager.BITWISE_BOSS;
    private void vulnerable(){
        // check if we have dashed into an enemy.
        Collider2D other = Physics2D.OverlapCircle(transform.position, 1, LayersManager.BITWISE_BOSS | LayersManager.BITWISE_ENEMY);
        if(other != null)
            handle_enemy_contact(other);
        col.excludeLayers = new LayerMask();
    }
    private void handle_enemy_contact(Collision2D other){
        if(other.gameObject.tag != "Dead" && i_frames == false)
            health.damage(new DamageData(1), new KnockbackData(10, 0.3f, other.transform));
    }
    private void handle_enemy_contact(Collider2D other){
        if(other.gameObject.tag != "Dead" && i_frames == false)
            health.damage(new DamageData(1), new KnockbackData(10, 0.3f, other.transform));
    } 
    private void damaged() => StartCoroutine(damaged_state());
    IEnumerator damaged_state(){
        sound.play_non_diegetic_one_shot("player_damaged");
        sprite.play_damaged_flash();
        health.set_invulnerable();
        AudioManager.enter_low_pass_filter();
        CameraController.instance.shake_camera(0.25f, 1, lock_shake: false);
        i_frames = true;
        damaged_start?.Invoke();
        yield return new WaitForSeconds(invulnerable_time);
        i_frames = false;
        AudioManager.exit_low_pass_filter();
        damaged_stop?.Invoke();
        health.set_vulnerable();
    }
    protected override void death_start() => StartCoroutine(death_state());
    IEnumerator death_state(){
        GameManager.state_changed(GameState.DEATH);
        unlink_movement();
        unlink_health();
        health.set_invulnerable();
        //AudioManager.low_pass_audio(true);
        movement.reset_data();
        movement.zero_velocity();
        CameraController.instance.shake_camera(0.25f, 1, lock_shake: false);
        sound.play_non_diegetic_one_shot("player_damaged");
        sprite.play_death_effect(3f);
        animator.death();
        InputManager.disable_user_input();
        base.death_start();
        yield return new WaitForSeconds(3);
        //AudioManager.low_pass_audio(false);
        base.death_complete();
    }
    private void healed(){
        // sound.play_sound("healed");
    }





    // Spawn & Door.
    private void queue_start_left()     { door_movement_queue.Add(start_left);     }
    private void dequeue_start_left()   { door_movement_queue.Remove(start_left);  }
    private void queue_start_right()    { door_movement_queue.Add(start_right);    }
    private void dequeue_start_right()  { door_movement_queue.Remove(start_right); }
    private void queue_start_jump()     { door_movement_queue.Add(start_jump);     }
    private void dequeue_start_jump()   { door_movement_queue.Remove(start_jump);  }

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
        ///store_move_direction();
        unlink_gameplay_input();
        entered_door?.Invoke();
        //link_door_input(); 
        transform.parent = null;
    }
    IEnumerator door_exit_state(SpawnPoint spawn){
        movement.clear_move_direction();//
        input_vector_to_door_input();   
        spawn.use_spawn();
        movement.movement(spawn.get_movement(), true);
        unlink_gameplay_input();
        unlink_door_input(); 
        link_door_input(); 
        exiting_door?.Invoke();
        yield return new WaitForSeconds(1);
        movement.movement(spawn.get_movement(), false);
        unlink_door_input(); 
        link_gameplay_input();
        foreach(Action action in door_movement_queue)
            action();
        door_movement_queue.Clear(); // here for relocation doors
        GameManager.state_changed(GameState.GAMEPLAY);
        exited_door?.Invoke();
        yield break;
    }
    //void store_move_direction() => move_direction = get_movement().get_move_direction_copy();
    void input_vector_to_door_input(){
        Vector2 move_direction = InputManager.get_user_input_vector();
        if(move_direction.x<0)
            queue_start_left();
        else if(move_direction.x>0)
            queue_start_right();
        if(move_direction.y>0)
            queue_start_jump();
    }





    // Melee
    public float get_intermediate_health() => intermediate_health; 
    void attack_hit(){
        sound.play_diegetic_one_shot("player_sword_hit");
        if(health.get_current_health() < health.get_max_health()){
            intermediate_health += .1f;
            if(intermediate_health >= 1f){
                intermediate_health = 0;
                health.heal(1);
                intermediate_health_updated?.Invoke();
            }
            else
                intermediate_health_gained?.Invoke();
        }  
    }





    // Game States.
    protected override void enter_cutscene_state(){
        movement.StopAllCoroutines(); // here to stop player from going vulnerable during dash_end.
        movement.renew();
        unlink_movement();
        animator.force_idle();
        health.set_invulnerable();
        audio_listener.enabled = false;
    }
    protected override void exit_cutscene_state(){
        movement.renew();
        link_movement();
        movement.move_only_state();
        health.set_vulnerable();
        audio_listener.enabled = true;
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
    public PlayerSpriteHandler get_sprite() => sprite; 





    // Linkage
    private void unloaded_scene(Scene s) => unlink_events(); 
    protected void link_events(){
        link_gameplay_input();
        link_melee();
        link_movement();
        link_health();
        link_game_manager();
        link_scene_manager();
    }
    protected void unlink_events(){
        unlink_movement();
        unlink_gameplay_input();
        unlink_door_input();
        unlink_melee();
        unlink_health();
        unlink_game_manager();
        unlink_scene_manager();
    }
    public void link_gameplay_input(){
        InputManager.user_jump_performed    += start_jump;
        InputManager.user_jump_canceled     += stop_jump;
        InputManager.user_left_performed    += start_left;
        InputManager.user_left_canceled     += stop_left;
        InputManager.user_right_performed   += start_right;
        InputManager.user_right_canceled    += stop_right;
        InputManager.user_attack_performed  += attack;
        InputManager.user_dash_performed    += dash; 
        InputManager.user_up_performed      += start_up;
        InputManager.user_up_canceled       += stop_up;
    }
    public void unlink_gameplay_input(){
        InputManager.user_jump_performed    -= start_jump;
        InputManager.user_jump_canceled     -= stop_jump;
        InputManager.user_left_performed    -= start_left;
        InputManager.user_left_canceled     -= stop_left;
        InputManager.user_right_performed   -= start_right;
        InputManager.user_right_canceled    -= stop_right;
        InputManager.user_attack_performed  -= attack;
        InputManager.user_dash_performed    -= dash;    
        InputManager.user_up_performed      -= start_up;
        InputManager.user_up_canceled       -= stop_up;
    }
    public void link_door_input(){
        InputManager.user_jump_performed    += queue_start_jump;
        InputManager.user_jump_canceled     += dequeue_start_jump;
        InputManager.user_left_performed    += queue_start_left;
        InputManager.user_left_canceled     += dequeue_start_left;
        InputManager.user_right_performed   += queue_start_right;
        InputManager.user_right_canceled    += dequeue_start_right;
    }
    public void unlink_door_input(){
        InputManager.user_jump_performed    -= queue_start_jump;
        InputManager.user_jump_canceled     -= dequeue_start_jump;
        InputManager.user_left_performed    -= queue_start_left;
        InputManager.user_left_canceled     -= dequeue_start_left;
        InputManager.user_right_performed   -= queue_start_right;
        InputManager.user_right_canceled    -= dequeue_start_right;
    }
    protected void link_movement(){
        CharacterMovement movement = get_movement() as CharacterMovement;
        movement.move_direction_changed += face_direction; 
        movement.move_direction_changed += movement_updated;
        movement.now_grounded           += grounded;
        movement.not_grounded           += not_grounded;
        movement.jumped                 += jumped;
        movement.dashed                 += dashed;
        movement.dash_end               += dash_end;
        movement.new_ground             += new_ground;
        flipped_left                    += movement.flip_left;
        flipped_right                   += movement.flip_right;
    }
    protected void unlink_movement(){
        CharacterMovement movement = get_movement() as CharacterMovement;
        movement.move_direction_changed -= face_direction; 
        movement.move_direction_changed -= movement_updated;
        movement.now_grounded           -= grounded;
        movement.not_grounded           -= not_grounded;
        movement.jumped                 -= jumped;
        movement.dashed                 -= dashed;
        movement.dash_end               -= dash_end;
        movement.new_ground             -= new_ground;
        flipped_left                    -= movement.flip_left;
        flipped_right                   -= movement.flip_right;
    }
    private void link_melee(){
        flipped_left            += particles.flip_particles_left;
        flipped_right           += particles.flip_particles_right;
        melee.hit               += attack_hit;
        melee.self_knockback    += movement.knockback_additive;
    }
    private void unlink_melee(){
        flipped_left            -= particles.flip_particles_left;
        flipped_right           -= particles.flip_particles_right;
        melee.hit               -= attack_hit;
        melee.self_knockback    -= movement.knockback_additive;
    }
    protected void link_health(){
        health.now_invulnerable += invulnerable;
        health.now_vulnerable   += vulnerable;
        health.damaged          += damaged;
        health.knockback        += get_movement().knockback;
        health.death            += kill;
        health.death            += get_movement().StopAllCoroutines;    
        health.healed           += healed;  
    }
    protected void unlink_health(){
        health.now_invulnerable -= invulnerable;
        health.now_vulnerable   -= vulnerable;
        health.damaged          -= damaged;
        health.knockback        -= get_movement().knockback;
        health.death            -= kill;
        health.death            -= get_movement().StopAllCoroutines;  
        health.healed           -= healed;  
    }
    protected void link_scene_manager(){
        SceneManager.sceneUnloaded += unloaded_scene;
        CustomSceneManager.loading_scene += unlink_events;
    }
    protected void unlink_scene_manager(){
        SceneManager.sceneUnloaded -= unloaded_scene;
        CustomSceneManager.loading_scene -= unlink_events;
    }
    protected override void link_game_manager(){
        GameManager.set_game_data += set_game_data;
        entered_game_state(GameManager.get_state());
        base.link_game_manager();
    }
    protected override void unlink_game_manager(){
        GameManager.set_game_data -= set_game_data;
        base.unlink_game_manager();
    }





}
