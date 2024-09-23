using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour{
    [Header("External References")]
    [SerializeField] private InputManager input;
    [Header("Internal References")]
    [SerializeField] private CharacterMovement movement;
    [SerializeField] private Interactor interactor;
    void Start(){   
        input.jump  += jump;
        input.left  += left;
        input.right += right;
        input.interact += interact;
    }

    void jump(InputAction.CallbackContext ctx){
        if(ctx.performed == true)
            movement.jump();
        else if(ctx.canceled == true)
            movement.end_jump();
    }

    void left(InputAction.CallbackContext ctx){
        movement.move_left(ctx.performed);
    }

    void right(InputAction.CallbackContext ctx){
        movement.move_right(ctx.performed);
    }

    void interact(InputAction.CallbackContext ctx){
        if(ctx.performed == true)
            interactor.interact();
    }
}
