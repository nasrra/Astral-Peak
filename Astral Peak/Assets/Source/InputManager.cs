using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

// Use Case:
// This class is used to encapsulate all input functionality.
// redirecting the flow of simplified inputs to listening objects.

public class InputManager : MonoBehaviour{
    public static InputManager instance;
    [SerializeField] private PlayerInput input;
    private Keybinds keybinds;

    public event Action
        // Default keyboard events
        jump_performed,     jump_cancelled, 
        left_performed,     left_cancelled, 
        right_performed,    right_cancelled, 
        interact_performed, interact_cancelled, 
        attack_performed,   attack_cancelled, 
        parry_performed,    parry_cancelled,
        dash_performed,
        exit_performed;

    void Awake() => instance = this;

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
        keybinds.Keyboard.Jump.performed        += on_jump_performed;
        keybinds.Keyboard.Jump.canceled         += on_jump_cancelled;
        keybinds.Keyboard.Right.performed       += on_right_performed;
        keybinds.Keyboard.Right.canceled        += on_right_cancelled;
        keybinds.Keyboard.Left.performed        += on_left_performed;
        keybinds.Keyboard.Left.canceled         += on_left_cancelled;
        keybinds.Keyboard.Interact.performed    += on_interact_performed;
        keybinds.Keyboard.Interact.canceled     += on_interact_cancelled;
        keybinds.Keyboard.Attack.performed      += on_attack_performed;
        keybinds.Keyboard.Attack.canceled       += on_attack_cancelled;
        keybinds.Keyboard.Parry.performed       += on_parry_performed;
        keybinds.Keyboard.Parry.canceled        += on_parry_cancelled;
        keybinds.Keyboard.Dash.performed        += on_dash_performed;
        keybinds.Keyboard.ZoomOut.performed     += on_zoom_out;
        keybinds.Keyboard.ZoomIn.performed      += on_zoom_in;
        keybinds.Keyboard.Exit.performed        += on_exit_performed;
    }

    private void unbind_default_keyboard(){
        keybinds.Keyboard.Jump.performed        -= on_jump_performed;
        keybinds.Keyboard.Jump.canceled         -= on_jump_cancelled;
        keybinds.Keyboard.Right.performed       -= on_right_performed;
        keybinds.Keyboard.Right.canceled        -= on_right_cancelled;
        keybinds.Keyboard.Left.performed        -= on_left_performed;
        keybinds.Keyboard.Left.canceled         -= on_left_cancelled;
        keybinds.Keyboard.Interact.performed    -= on_interact_performed;
        keybinds.Keyboard.Interact.canceled     -= on_interact_cancelled;
        keybinds.Keyboard.Attack.performed      -= on_attack_performed;
        keybinds.Keyboard.Attack.canceled       -= on_attack_cancelled;
        keybinds.Keyboard.Parry.performed       -= on_parry_performed;
        keybinds.Keyboard.Parry.canceled        -= on_parry_cancelled;
        keybinds.Keyboard.Dash.performed        -= on_dash_performed;
        keybinds.Keyboard.ZoomOut.performed     -= on_zoom_out;
        keybinds.Keyboard.ZoomIn.performed      -= on_zoom_in;
        keybinds.Keyboard.Exit.performed        -= on_exit_performed;
    }
    void on_jump_performed(InputAction.CallbackContext ctx)     => jump_performed?.Invoke();
    void on_jump_cancelled(InputAction.CallbackContext ctx)     => jump_cancelled?.Invoke();
    void on_left_performed(InputAction.CallbackContext ctx)     => left_performed?.Invoke();
    void on_left_cancelled(InputAction.CallbackContext ctx)     => left_cancelled?.Invoke();
    void on_right_performed(InputAction.CallbackContext ctx)    => right_performed?.Invoke();
    void on_right_cancelled(InputAction.CallbackContext ctx)    => right_cancelled?.Invoke();
    void on_interact_performed(InputAction.CallbackContext ctx) => interact_performed?.Invoke();
    void on_interact_cancelled(InputAction.CallbackContext ctx) => interact_cancelled?.Invoke();
    void on_attack_performed(InputAction.CallbackContext ctx)   => attack_performed?.Invoke();
    void on_attack_cancelled(InputAction.CallbackContext ctx)   => attack_cancelled?.Invoke();
    void on_parry_performed(InputAction.CallbackContext ctx)    => parry_performed?.Invoke();
    void on_parry_cancelled(InputAction.CallbackContext ctx)    => parry_cancelled?.Invoke();
    void on_dash_performed(InputAction.CallbackContext ctx)     => dash_performed?.Invoke();
    void on_exit_performed(InputAction.CallbackContext ctx)     => exit_performed?.Invoke();
    void on_zoom_out(InputAction.CallbackContext ctx)           => CameraController.instance.ZoomOut();
    void on_zoom_in(InputAction.CallbackContext ctx)            => CameraController.instance.ZoomIn();
    #endregion
}
