
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Creature{
    
    // static fields for other classes to access.
    public static Player player;
    public static string exit_point = "";

    // data to link together.
    [Header("Player")]
    [SerializeField] private InputManager input;
    [SerializeField] private CharacterMovement movement;
    [SerializeField] private Interactor interactor;

    void Awake(){
        player = this;
    }

    void Start(){   
        link_input();
        link_events();
        set_enter_position();
        // snap camera to players new position.
        CameraController.instance.snap_to_target(); 
    }

    void OnDestroy(){
        unlink_input();
        unlink_events();
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

    private void link_events(){
        movement.start_knockback += unlink_input;
        movement.end_knockback += link_input;
    }

    private void unlink_events(){
        movement.start_knockback -= unlink_input;
        movement.end_knockback -= link_input;        
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

    // used to set the players initial position in the scene.
    public void set_enter_position(){
        if(exit_point != "")
            transform.position = DoorManager.instance.get_position(exit_point);
    }

    void OnCollisionEnter2D(Collision2D other){  
        if(other.gameObject.tag == "Enemy")
            movement.knockback(transform.position - other.transform.position, 6, 0.25f);
    }
}
