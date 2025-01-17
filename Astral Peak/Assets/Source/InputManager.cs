using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor;
using Entropek;

// Use Case:
// This class is used to encapsulate all input functionality.
// redirecting the flow of simplified inputs to listening objects.

public static class InputManager{
    private static PlayerInput input;
    private static Keybinds keybinds = new Keybinds();
    private static InputActionRebindingExtensions.RebindingOperation rebinding_operation;

    public static event Action
        // Default keyboard events
        jump_performed,     jump_cancelled, 
        left_performed,     left_cancelled, 
        right_performed,    right_cancelled, 
        attack_performed,   attack_cancelled, 
        up_performed,       up_cancelled,
        dash_performed;

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

    private static readonly Dictionary<GameState, Action> enable_input = new Dictionary<GameState, Action>(){
        {GameState.GAMEPLAY, enable_user_input},
        {GameState.MENU, enable_menu_input},
    };

    private static readonly Dictionary<GameState, Action> disable_input = new Dictionary<GameState, Action>(){
        {GameState.GAMEPLAY, disable_user_input},
        {GameState.MENU, disable_menu_input},
    };

    // Reset the input blockers
    public static void reset_input_blockers(){
        foreach (Actions action in input_blocker.Keys.ToList())
            input_blocker[action] = true;
    }

    public static void initialize(PlayerInput _input){
        input = _input;
        // enable keyboard keybinds
        keybinds.UserControls.Enable();
        // bind
        bind_keybinds();
        GameManager.exited_game_state  += exited_game_state;
        GameManager.entered_game_state += entered_game_state;
    }
    public static void uninitialize(){
        unbind_keybinds();
        GameManager.exited_game_state  -= exited_game_state;
        GameManager.entered_game_state -= entered_game_state;
    }




    // Linkage: 
    private static void bind_keybinds(){
        link_user_controls();
        link_rebind_controls();
        link_menu_controls();
    }
    private static void link_user_controls(){
        keybinds.UserControls.Jump.performed            += user_jump_performed;
        keybinds.UserControls.Jump.canceled             += user_jump_cancelled;
        keybinds.UserControls.Right.performed           += user_right_performed;
        keybinds.UserControls.Right.canceled            += user_right_cancelled;
        keybinds.UserControls.Left.performed            += user_left_performed;
        keybinds.UserControls.Left.canceled             += user_left_cancelled;
        keybinds.UserControls.Up.performed              += user_up_performed;
        keybinds.UserControls.Up.canceled               += user_up_cancelled;
        keybinds.UserControls.Attack.performed          += user_attack_performed;
        keybinds.UserControls.Attack.canceled           += user_attack_cancelled;
        keybinds.UserControls.Dash.performed            += user_dash_performed;
        keybinds.UserControls.Exit.performed            += user_exit_performed;
        #if UNITY_EDITOR
            keybinds.UserControls.ZoomOut.performed     += user_zoom_out;
            keybinds.UserControls.ZoomIn.performed      += user_zoom_in;
        #endif
    }
    private static void link_rebind_controls(){
        keybinds.RebindControls.Cancel.performed        += rebind_cancel_performed;
    }
    private static void link_menu_controls(){
        keybinds.MenuControls.Exit.performed            += menu_exit_performed;
    }
    private static void unbind_keybinds(){
        unlink_user_controls();
        unlink_rebind_controls();
        unlink_menu_controls();
    }
    private static void unlink_user_controls(){
        keybinds.UserControls.Jump.performed            -= user_jump_performed;
        keybinds.UserControls.Jump.canceled             -= user_jump_cancelled;
        keybinds.UserControls.Right.performed           -= user_right_performed;
        keybinds.UserControls.Right.canceled            -= user_right_cancelled;
        keybinds.UserControls.Left.performed            -= user_left_performed;
        keybinds.UserControls.Left.canceled             -= user_left_cancelled;
        keybinds.UserControls.Attack.performed          -= user_attack_performed;
        keybinds.UserControls.Attack.canceled           -= user_attack_cancelled;
        keybinds.UserControls.Dash.performed            -= user_dash_performed;
        keybinds.UserControls.Exit.performed            -= user_exit_performed;
        #if UNITY_EDITOR
            keybinds.UserControls.ZoomOut.performed     -= user_zoom_out;
            keybinds.UserControls.ZoomIn.performed      -= user_zoom_in;
        #endif
    }
    private static void unlink_rebind_controls(){
        keybinds.RebindControls.Cancel.performed        -= rebind_cancel_performed;
    }
    private static void unlink_menu_controls(){
        keybinds.MenuControls.Exit.performed            -= menu_exit_performed;
    }





    // GameState:
    static void entered_game_state(GameState state){
        if(enable_input.ContainsKey(state))
            enable_input[state]();
    }
    static void exited_game_state(GameState state){
        if(disable_input.ContainsKey(state))
            disable_input[state]();
    } 





    // User Controls:
    public static void enable_user_input(){
        keybinds.UserControls.Enable();
    }
    public static void disable_user_input(){
        keybinds.UserControls.Disable();
        reset_input_blockers();
    }
    static void user_jump_performed(InputAction.CallbackContext ctx)    { jump_performed?.Invoke(); input_blocker[Actions.JUMP] = false;}
    static void user_jump_cancelled(InputAction.CallbackContext ctx)    { if(input_blocker[Actions.JUMP] == false) jump_cancelled?.Invoke();}
    static void user_left_performed(InputAction.CallbackContext ctx)    { left_performed?.Invoke(); input_blocker[Actions.LEFT] = false;}
    static void user_left_cancelled(InputAction.CallbackContext ctx)    { if(input_blocker[Actions.LEFT] == false) left_cancelled?.Invoke();}
    static void user_right_performed(InputAction.CallbackContext ctx)   { right_performed?.Invoke(); input_blocker[Actions.RIGHT] = false;}
    static void user_right_cancelled(InputAction.CallbackContext ctx)   { if(input_blocker[Actions.RIGHT] == false) right_cancelled?.Invoke();}
    static void user_attack_performed(InputAction.CallbackContext ctx)  { attack_performed?.Invoke(); input_blocker[Actions.ATTACK] = false;}
    static void user_attack_cancelled(InputAction.CallbackContext ctx)  { if(input_blocker[Actions.ATTACK] == false) attack_cancelled?.Invoke();}
    static void user_up_performed(InputAction.CallbackContext ctx)      { up_performed?.Invoke(); input_blocker[Actions.UP] = false;}
    static void user_up_cancelled(InputAction.CallbackContext ctx)      { if(input_blocker[Actions.UP] == false) up_cancelled?.Invoke();}
    static void user_dash_performed(InputAction.CallbackContext ctx)    => dash_performed?.Invoke();
    static void user_exit_performed(InputAction.CallbackContext ctx)    => UiManager.instance.toggle_gameplay_ui();
    #if UNITY_EDITOR
        static void user_zoom_out(InputAction.CallbackContext ctx)      => CameraController.instance?.ZoomOut();
        static void user_zoom_in(InputAction.CallbackContext ctx)       => CameraController.instance?.ZoomIn();
    #endif
    




    // Pause Menu Controls: 
    public static void enable_menu_input(){
        keybinds.MenuControls.Enable();
    }
    public static void disable_menu_input(){
        keybinds.MenuControls.Disable();
    } 
    static void menu_exit_performed(InputAction.CallbackContext ctx)   => UiManager.instance?.toggle_gameplay_ui();





    // Rebind Controls:
    static void rebind_cancel_performed(InputAction.CallbackContext ctx)       => cancel_rebind();
    public static void rebind_action(InputAction action, Action callback = null){        
        keybinds.MenuControls.Disable();
        keybinds.RebindControls.Enable();
        string original_binding = action.bindings[0].effectivePath;  // Store the original binding
        rebinding_operation = action.PerformInteractiveRebinding()
        .WithControlsExcluding("Mouse")
        .WithControlsExcluding("<Keyboard>/Escape")
        .WithControlsExcluding("<Keyboard>/anyKey")
        .OnCancel(c =>{
            Debug.Log("Rebinding operation was canceled.");
            c.Dispose();
            keybinds.MenuControls.Enable();
            keybinds.RebindControls.Disable();
        })
        .OnComplete(c =>{
            Debug.Log("Rebind successful!");
            c.Dispose();
            keybinds.MenuControls.Enable();
            keybinds.RebindControls.Disable();
            callback?.Invoke();
        });
        rebinding_operation.Start();
    }
    private static void cancel_rebind(){
        rebinding_operation.Cancel();
    }





    // Util:
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
