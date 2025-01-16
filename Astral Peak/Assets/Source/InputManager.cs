using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

// Use Case:
// This class is used to encapsulate all input functionality.
// redirecting the flow of simplified inputs to listening objects.

public static class InputManager{
    private static PlayerInput input;
    private static Keybinds keybinds;

    public static event Action
        // Default keyboard events
        jump_performed,     jump_cancelled, 
        left_performed,     left_cancelled, 
        right_performed,    right_cancelled, 
        attack_performed,   attack_cancelled, 
        up_performed,       up_cancelled,
        dash_performed,
        exit_performed;
        //debug_performed;

    enum Actions{
        JUMP,
        LEFT,
        RIGHT,
        UP,
        ATTACK,
    }

    static Dictionary<Actions, bool> input_blocker = new Dictionary<Actions, bool>(){
        {Actions.JUMP,    true},   
        {Actions.LEFT,    true},
        {Actions.RIGHT,   true},
        {Actions.ATTACK,  true},
        {Actions.UP,      true},
    };

    // Reset the input blockers
    public static void reset_input_blockers(){
        foreach (Actions action in input_blocker.Keys.ToList())
            input_blocker[action] = true;
    }

    public static void initialize(PlayerInput _input){
        input = _input;
        // enable keyboard keybinds
        keybinds = new Keybinds();
        keybinds.UserControls.Enable();
        // bind
        bind_default_keyboard();
    }

    private static void bind_default_keyboard(){
        keybinds.UserControls.Jump.performed            += on_jump_performed;
        keybinds.UserControls.Jump.canceled             += on_jump_cancelled;
        keybinds.UserControls.Right.performed           += on_right_performed;
        keybinds.UserControls.Right.canceled            += on_right_cancelled;
        keybinds.UserControls.Left.performed            += on_left_performed;
        keybinds.UserControls.Left.canceled             += on_left_cancelled;
        keybinds.UserControls.Up.performed              += on_up_performed;
        keybinds.UserControls.Up.canceled               += on_up_cancelled;
        keybinds.UserControls.Attack.performed          += on_attack_performed;
        keybinds.UserControls.Attack.canceled           += on_attack_cancelled;
        keybinds.UserControls.Dash.performed            += on_dash_performed;
        keybinds.UserControls.ZoomOut.performed         += on_zoom_out;
        keybinds.UserControls.ZoomIn.performed          += on_zoom_in;
        keybinds.UserControls.Exit.performed            += on_exit_performed;
        keybinds.UserControls.Debug.performed           += on_debug_performed;
    }

    private static void unbind_default_keyboard(){
        keybinds.UserControls.Jump.performed            -= on_jump_performed;
        keybinds.UserControls.Jump.canceled             -= on_jump_cancelled;
        keybinds.UserControls.Right.performed           -= on_right_performed;
        keybinds.UserControls.Right.canceled            -= on_right_cancelled;
        keybinds.UserControls.Left.performed            -= on_left_performed;
        keybinds.UserControls.Left.canceled             -= on_left_cancelled;
        keybinds.UserControls.Attack.performed          -= on_attack_performed;
        keybinds.UserControls.Attack.canceled           -= on_attack_cancelled;
        keybinds.UserControls.Dash.performed            -= on_dash_performed;
        keybinds.UserControls.ZoomOut.performed         -= on_zoom_out;
        keybinds.UserControls.ZoomIn.performed          -= on_zoom_in;
        keybinds.UserControls.Exit.performed            -= on_exit_performed;
        keybinds.UserControls.Debug.performed           -= on_debug_performed;
    }
    static void on_jump_performed(InputAction.CallbackContext ctx)          { jump_performed?.Invoke(); input_blocker[Actions.JUMP] = false;}
    static void on_jump_cancelled(InputAction.CallbackContext ctx)          { if(input_blocker[Actions.JUMP] == false) jump_cancelled?.Invoke();}
    static void on_left_performed(InputAction.CallbackContext ctx)          { left_performed?.Invoke(); input_blocker[Actions.LEFT] = false;}
    static void on_left_cancelled(InputAction.CallbackContext ctx)          { if(input_blocker[Actions.LEFT] == false) left_cancelled?.Invoke();}
    static void on_right_performed(InputAction.CallbackContext ctx)         { right_performed?.Invoke(); input_blocker[Actions.RIGHT] = false;}
    static void on_right_cancelled(InputAction.CallbackContext ctx)         { if(input_blocker[Actions.RIGHT] == false) right_cancelled?.Invoke();}
    static void on_attack_performed(InputAction.CallbackContext ctx)        { attack_performed?.Invoke(); input_blocker[Actions.ATTACK] = false;}
    static void on_attack_cancelled(InputAction.CallbackContext ctx)        { if(input_blocker[Actions.ATTACK] == false) attack_cancelled?.Invoke();}
    static void on_up_performed(InputAction.CallbackContext ctx)            { up_performed?.Invoke(); input_blocker[Actions.UP] = false;}
    static void on_up_cancelled(InputAction.CallbackContext ctx)            { if(input_blocker[Actions.UP] == false) up_cancelled?.Invoke();}
    static void on_dash_performed(InputAction.CallbackContext ctx)          => dash_performed?.Invoke();
    static void on_exit_performed(InputAction.CallbackContext ctx)          => exit_performed?.Invoke();
    static void on_zoom_out(InputAction.CallbackContext ctx)                => CameraController.instance?.ZoomOut();
    static void on_zoom_in(InputAction.CallbackContext ctx)                 => CameraController.instance?.ZoomIn();
    static void on_debug_performed(InputAction.CallbackContext ctx)         => UiManager.instance?.start_dialogue();

    public static InputAction get_input_action(string input_action){
        InputActionMap actionMap = keybinds.UserControls;
        InputAction action = actionMap.FindAction(input_action);
        #if UNITY_EDITOR
        if(action==null)
            throw new NullReferenceException($"Input Action: '{input_action}' not found");
        #endif
        return action;
    }
    public static Sprite get_input_binding_image(InputAction action, int binding){
        string key_name = action.bindings[binding].ToDisplayString()+"_Key_Light";

        #if UNITY_EDITOR
            // Editor-only validation to check if the file exists
            string resource_path = $"Sprites/Keybinds/Light/{key_name}";
            string asset_path = $"Assets/Resources/{resource_path}.png"; // Adjust extension as needed
            if (!System.IO.File.Exists(asset_path))
                throw new NullReferenceException($"Input binding image not found at: {asset_path}");
        #endif
        return Resources.Load<Sprite>("Sprites/Keybinds/Light/"+key_name);
    }
}
