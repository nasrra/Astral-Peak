using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Creature{
    
    public static Player player;
    [Header("Player")]
    [SerializeField] private InputManager input;
    [SerializeField] private CharacterMovement movement;
    [SerializeField] private Interactor interactor;

    void Start(){   
        player = this;
        link_input();
    }

    void OnDestroy(){
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
        if(ctx.canceled == true)
            animator.SetTrigger("idle");
    }

    private void parry(InputAction.CallbackContext ctx){
        if(ctx.performed == true)
            animator.SetTrigger("parry");
        if(ctx.canceled == true)
            animator.SetTrigger("idle");
    }
}
