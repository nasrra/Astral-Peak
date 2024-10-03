

using System.Runtime.InteropServices;
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

    void Awake(){
        player = this;
    }

    void Start(){   
        link_events();
        link_input();
        set_enter_position();
        // snap camera to players new position.
        CameraController.instance.snap_to_target(); 
    }

    void OnDestroy(){
        unlink_events();
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

    private void left(InputAction.CallbackContext ctx) => movement.move_left(ctx.performed);
    private void right(InputAction.CallbackContext ctx) => movement.move_right(ctx.performed);

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
}
