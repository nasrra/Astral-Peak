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
    public event InputDelegate interact;
    public event InputDelegate attack;

    private Keybinds keybinds;
    void Start(){
        // enable keyboard keybinds
        keybinds = new Keybinds();
        keybinds.Keyboard.Enable();
        // bind
        bind_default_keyboard();
    }

    void OnDestroy(){
        //unbind    
        unbind_default_keyboard();
    }

    #region Default Keyboard
    private void bind_default_keyboard(){
        keybinds.Keyboard.Jump.performed        += OnJump;
        keybinds.Keyboard.Jump.canceled         += OnJump;
        keybinds.Keyboard.Right.performed       += OnRight;
        keybinds.Keyboard.Right.canceled        += OnRight;
        keybinds.Keyboard.Left.performed        += OnLeft;
        keybinds.Keyboard.Left.canceled         += OnLeft;
        keybinds.Keyboard.Interact.performed    += OnInteract;
        keybinds.Keyboard.Interact.canceled     += OnInteract;
        keybinds.Keyboard.Attack.performed      += OnAttack;
        keybinds.Keyboard.Attack.canceled       += OnAttack;
    }

    private void unbind_default_keyboard(){
        keybinds.Keyboard.Jump.performed        -= OnJump;
        keybinds.Keyboard.Jump.canceled         -= OnJump;
        keybinds.Keyboard.Right.performed       -= OnRight;
        keybinds.Keyboard.Right.canceled        -= OnRight;
        keybinds.Keyboard.Left.performed        -= OnLeft;
        keybinds.Keyboard.Left.canceled         -= OnLeft;
        keybinds.Keyboard.Interact.performed    -= OnInteract;
        keybinds.Keyboard.Interact.canceled     -= OnInteract;
        keybinds.Keyboard.Attack.performed      -= OnAttack;
        keybinds.Keyboard.Attack.canceled       -= OnAttack; 
    }
    void OnJump(InputAction.CallbackContext ctx) => jump?.Invoke(ctx);
    void OnLeft(InputAction.CallbackContext ctx) => left?.Invoke(ctx);
    void OnRight(InputAction.CallbackContext ctx) => right?.Invoke(ctx);
    void OnInteract(InputAction.CallbackContext ctx) => interact?.Invoke(ctx);
    void OnAttack(InputAction.CallbackContext ctx) => attack?.Invoke(ctx);
    #endregion
}
