using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour{
    [Header("External References")]
    [SerializeField] private InputManager input;
    [Header("Internal References")]
    [SerializeField] private Movement movement;
    void Start(){   
        input.jump += jump;
        input.left += left;
        input.right += right;
    }

    void jump(InputAction.CallbackContext ctx){
        Debug.Log("p jump!");
        movement.move_up((ctx.performed == true));
    }

    void left(InputAction.CallbackContext ctx){
        Debug.Log("p left!");
        movement.move_left(ctx.performed);
    }

    void right(InputAction.CallbackContext ctx){
        Debug.Log("p right!");
        movement.move_right(ctx.performed);
    }
}
