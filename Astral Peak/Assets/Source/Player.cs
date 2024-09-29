
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Creature{
    
    // static fields for other classes to access.
    public static Player player;
    public static string exit_point = "";

    // data to link together.
    [Header("Player")]
    [SerializeField] private bool parrying = false;
    [SerializeField] private InputManager input;
    [SerializeField] private CharacterMovement movement;
    [SerializeField] private Interactor interactor;

    void Awake(){
        player = this;
    }

    void Start(){   
        //link_events();
        link_input();
        set_enter_position();
        // snap camera to players new position.
        CameraController.instance.snap_to_target(); 
    }

    void OnDestroy(){
        //unlink_events();
        unlink_input();
    }

    private void link_input(){
        input.jump      += jump;
        input.left      += left;
        input.right     += right;
        input.interact  += interact; 
        input.attack    += attack;  
        input.parry     += parry;     
    }

    private void unlink_input(){
        input.jump      -= jump;
        input.left      -= left;
        input.right     -= right;
        input.interact  -= interact;
        input.attack    -= attack;  
        input.parry     -= parry;         
    }

    protected override void link_events(){
        base.link_events();
        health.on_invulnerable += parry_check;
    }

    protected override void unlink_events(){
        base.unlink_events();
        health.on_invulnerable -= parry_check;
    }


    void OnCollisionEnter2D(Collision2D other){  
        if(other.gameObject.tag == "Enemy")
            handle_enemy_contact(other);
    }

    private void jump(InputAction.CallbackContext ctx){
        if(ctx.performed == true)
            movement.jump();
        else if(ctx.canceled == true)
            movement.end_jump();
    }

    private void left(InputAction.CallbackContext ctx){
        movement.move_left(ctx.performed);
        face_move_dir();
    }

    private void right(InputAction.CallbackContext ctx){
        movement.move_right(ctx.performed);
        face_move_dir();
    }

    // flip the player in relation to where they are moving towards.
    private void face_move_dir(){
        if(movement.get_move_direction().x < 0)
            flip_left();
        if(movement.get_move_direction().x > 0)
            flip_right();
    }

    private void interact(InputAction.CallbackContext ctx){
        if(ctx.performed == true)
            interactor.interact();
    }

    private void attack(InputAction.CallbackContext ctx){
        if(ctx.performed == true)
            animator.SetTrigger("attack");
    }

    private void parry(InputAction.CallbackContext ctx){
        if(ctx.performed == true)
            animator.SetTrigger("parry");
    }

    private void parry_check(){
        if(parrying == true)
            Debug.Log("parried!");
    }

    // used to set the players initial position in the scene.
    public void set_enter_position(){
        if(exit_point != "")
            transform.position = DoorManager.instance.get_position(exit_point);
    }

    public void is_parrying(int x){
        parrying = x != 0;
        health.set_invulnerable(x);
    }


    private void handle_enemy_contact(Collision2D other){
        if(parrying == false){
            // knockback the player.    
            movement.knockback(transform.position - other.transform.position, 10, 0.3f);
            // damage the player.
            health.damage(1);
        }
        else
            other.gameObject.GetComponent<Movement>().knockback(other.transform.position - transform.position, 10, 0.3f);
    }
}
