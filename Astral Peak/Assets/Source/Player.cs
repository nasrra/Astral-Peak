using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Player : MonoBehaviour{
    [Header("External References")]
    [SerializeField] private InputManager input;
    [Header("Internal References")]
    [SerializeField] private CharacterMovement movement;
    void Start(){   
        input.jump += jump;
        input.left += left;
        input.right += right;
    }

    void jump(InputAction.CallbackContext ctx){
        movement.move_up(ctx.performed);
    }

    void left(InputAction.CallbackContext ctx){
        movement.move_left(ctx.performed);
    }

    void right(InputAction.CallbackContext ctx){
        movement.move_right(ctx.performed);
    }
}
