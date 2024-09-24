using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour{
    public static Player player;
    [Header("External References")]
    [SerializeField] private InputManager input;
    [Header("Internal References")]
    [SerializeField] private CharacterMovement movement;
    [SerializeField] private Interactor interactor;
    [SerializeField] private Health health;
    void Start(){   
        player = this;
        input.jump  += jump;
        input.left  += left;
        input.right += right;
        input.interact += interact;
    }

    public Health get_health(){
        return health;
    }

    private void jump(InputAction.CallbackContext ctx){
        if(ctx.performed == true)
            movement.jump();
        else if(ctx.canceled == true)
            movement.end_jump();
    }

    private void left(InputAction.CallbackContext ctx){
        movement.move_left(ctx.performed);
    }

    private void right(InputAction.CallbackContext ctx){
        movement.move_right(ctx.performed);
    }

    private void interact(InputAction.CallbackContext ctx){
        if(ctx.performed == true)
            interactor.interact();
    }
}
