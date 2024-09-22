using UnityEngine;
using UnityEngine.InputSystem;

// Use Case:
// This class is used to encapsulate all input functionality.
// redirecting the flow of simplified inputs to listening objects.

public class InputManager : MonoBehaviour{
    [SerializeField] private PlayerInput input;
    public delegate void InputDelegate (InputAction.CallbackContext ctx);
    public event InputDelegate jump;
    public event InputDelegate left;
    public event InputDelegate right;

    private Keybinds keybinds;
    void Start(){
        // enable keyboard keybinds
        keybinds = new Keybinds();
        keybinds.Keyboard.Enable();
        // bind
        keybinds.Keyboard.Jump.performed += OnJump;
        keybinds.Keyboard.Jump.canceled += OnJump;
        keybinds.Keyboard.Right.performed += OnRight;
        keybinds.Keyboard.Right.canceled += OnRight;
        keybinds.Keyboard.Left.performed += OnLeft;
        keybinds.Keyboard.Left.canceled += OnLeft;
    }

    void OnJump(InputAction.CallbackContext ctx) => jump?.Invoke(ctx);
    void OnLeft(InputAction.CallbackContext ctx) => left?.Invoke(ctx);
    void OnRight(InputAction.CallbackContext ctx) => right?.Invoke(ctx);

    void OnDestroy(){
        //unbind
        keybinds.Keyboard.Jump.performed -= OnJump;
        keybinds.Keyboard.Right.performed -= OnRight;
        keybinds.Keyboard.Left.performed -= OnLeft;      
    }
}
