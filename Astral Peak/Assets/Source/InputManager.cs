using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Rendering;

// Use Case:
// This class is used to encapsulate all input functionality.
// redirecting the flow of simplified inputs to listening objects.

public static class InputManager{
    private static PlayerInput input;
    private static Keybinds keybinds = new Keybinds();
    private static InputActionRebindingExtensions.RebindingOperation rebinding_operation;

    public static event Action
        // Default keyboard events
        user_jump_performed,     user_jump_canceled, 
        user_left_performed,     user_left_canceled, 
        user_right_performed,    user_right_canceled, 
        user_attack_performed,   user_attack_canceled, 
        user_up_performed,       user_up_canceled,
        user_dash_performed,
        cutscene_skip_performed, cutscene_skip_canceled,
        menu_exit_performed;

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
        {GameState.CUTSCENE, enable_cutscene_input}
    };

    private static readonly Dictionary<GameState, Action> disable_input = new Dictionary<GameState, Action>(){
        {GameState.GAMEPLAY, disable_user_input},
        {GameState.MENU, disable_menu_input},
        {GameState.CUTSCENE, disable_cutscene_input}
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
        load_player_prefs(keybinds.UserControls);
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
        link_cutscene_controls();
    }
    private static void unbind_keybinds(){
        unlink_user_controls();
        unlink_rebind_controls();
        unlink_menu_controls();
        unlink_cutscene_controls();
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
    private static void link_user_controls(){
        keybinds.UserControls.Jump.performed            += on_user_jump_performed;
        keybinds.UserControls.Jump.canceled             += on_user_jump_canceled;
        keybinds.UserControls.Right.performed           += on_user_right_performed;
        keybinds.UserControls.Right.canceled            += on_user_right_canceled;
        keybinds.UserControls.Left.performed            += on_user_left_performed;
        keybinds.UserControls.Left.canceled             += on_user_left_canceled;
        keybinds.UserControls.Up.performed              += on_user_up_performed;
        keybinds.UserControls.Up.canceled               += on_user_up_canceled;
        keybinds.UserControls.Attack.performed          += on_user_attack_performed;
        keybinds.UserControls.Attack.canceled           += on_user_attack_canceled;
        keybinds.UserControls.Dash.performed            += on_user_dash_performed;
        keybinds.UserControls.Exit.performed            += on_user_exit_performed;
        #if UNITY_EDITOR
            keybinds.UserControls.ZoomOut.performed     += on_user_zoom_out;
            keybinds.UserControls.ZoomIn.performed      += on_user_zoom_in;
        #endif
    }
    private static void unlink_user_controls(){
        keybinds.UserControls.Jump.performed            -= on_user_jump_performed;
        keybinds.UserControls.Jump.canceled             -= on_user_jump_canceled;
        keybinds.UserControls.Right.performed           -= on_user_right_performed;
        keybinds.UserControls.Right.canceled            -= on_user_right_canceled;
        keybinds.UserControls.Left.performed            -= on_user_left_performed;
        keybinds.UserControls.Left.canceled             -= on_user_left_canceled;
        keybinds.UserControls.Attack.performed          -= on_user_attack_performed;
        keybinds.UserControls.Attack.canceled           -= on_user_attack_canceled;
        keybinds.UserControls.Dash.performed            -= on_user_dash_performed;
        keybinds.UserControls.Exit.performed            -= on_user_exit_performed;
        #if UNITY_EDITOR
            keybinds.UserControls.ZoomOut.performed     -= on_user_zoom_out;
            keybinds.UserControls.ZoomIn.performed      -= on_user_zoom_in;
        #endif
    }
    static void on_user_jump_performed(InputAction.CallbackContext ctx)    { user_jump_performed?.Invoke(); input_blocker[Actions.JUMP] = false;}
    static void on_user_jump_canceled(InputAction.CallbackContext ctx)    { if(input_blocker[Actions.JUMP] == false) user_jump_canceled?.Invoke();}
    static void on_user_left_performed(InputAction.CallbackContext ctx)    { user_left_performed?.Invoke(); input_blocker[Actions.LEFT] = false;}
    static void on_user_left_canceled(InputAction.CallbackContext ctx)    { if(input_blocker[Actions.LEFT] == false) user_left_canceled?.Invoke();}
    static void on_user_right_performed(InputAction.CallbackContext ctx)   { user_right_performed?.Invoke(); input_blocker[Actions.RIGHT] = false;}
    static void on_user_right_canceled(InputAction.CallbackContext ctx)   { if(input_blocker[Actions.RIGHT] == false) user_right_canceled?.Invoke();}
    static void on_user_attack_performed(InputAction.CallbackContext ctx)  { user_attack_performed?.Invoke(); input_blocker[Actions.ATTACK] = false;}
    static void on_user_attack_canceled(InputAction.CallbackContext ctx)  { if(input_blocker[Actions.ATTACK] == false) user_attack_canceled?.Invoke();}
    static void on_user_up_performed(InputAction.CallbackContext ctx)      { user_up_performed?.Invoke(); input_blocker[Actions.UP] = false;}
    static void on_user_up_canceled(InputAction.CallbackContext ctx)      { if(input_blocker[Actions.UP] == false) user_up_canceled?.Invoke();}
    static void on_user_dash_performed(InputAction.CallbackContext ctx)    => user_dash_performed?.Invoke();
    static void on_user_exit_performed(InputAction.CallbackContext ctx)    => UiManager.instance.toggle_gameplay_ui();
    #if UNITY_EDITOR
        static void on_user_zoom_out(InputAction.CallbackContext ctx)      => CameraController.instance?.ZoomOut();
        static void on_user_zoom_in(InputAction.CallbackContext ctx)       => CameraController.instance?.ZoomIn();
    #endif
    




    // Pause Menu Controls: 
    public static void enable_menu_input(){
        keybinds.MenuControls.Enable();
    }
    public static void disable_menu_input(){
        keybinds.MenuControls.Disable();
    } 
    private static void link_menu_controls(){
        keybinds.MenuControls.Exit.performed            += on_menu_exit_performed;
    }
    private static void unlink_menu_controls(){
        keybinds.MenuControls.Exit.performed            -= on_menu_exit_performed;
    }
    static void on_menu_exit_performed(InputAction.CallbackContext ctx)   => menu_exit_performed?.Invoke();





    // Cutscene Controls:
    public static void enable_cutscene_input(){
        keybinds.CutsceneControls.Enable();
    }
    public static void disable_cutscene_input(){
        keybinds.CutsceneControls.Disable();
    } 
    static void link_cutscene_controls(){
        keybinds.CutsceneControls.Skip.performed += on_custcene_skip_performed;
        keybinds.CutsceneControls.Skip.canceled  += on_custcene_skip_canceled;
    }
    static void unlink_cutscene_controls(){
        keybinds.CutsceneControls.Skip.performed -= on_custcene_skip_performed;
        keybinds.CutsceneControls.Skip.canceled  -= on_custcene_skip_canceled;
    }
    static void on_custcene_skip_performed(InputAction.CallbackContext ctx) =>  cutscene_skip_performed?.Invoke();
    static void on_custcene_skip_canceled(InputAction.CallbackContext ctx) =>  cutscene_skip_canceled?.Invoke();



    // Rebind Controls:
    private static void link_rebind_controls(){
        keybinds.RebindControls.Cancel.performed        += rebind_cancel_performed;
    }
    private static void unlink_rebind_controls(){
        keybinds.RebindControls.Cancel.performed        -= rebind_cancel_performed;
    }
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
            
            // Save the rebinds
            save_action_map(action.actionMap);            
            callback?.Invoke();
        });
        rebinding_operation.Start();
    }
    private static void cancel_rebind(){
        rebinding_operation.Cancel();
    }
    private static void load_player_prefs(InputActionMap action_map) {
        string overrides = PlayerPrefs.GetString("Rebinds_" + action_map.name, string.Empty);
        if (!string.IsNullOrEmpty(overrides))
            action_map.LoadBindingOverridesFromJson(overrides);
    }
    public static void reset_keybinds(string _action_map){
        InputActionMap action_map = keybinds.asset.FindActionMap(_action_map); 
        action_map.RemoveAllBindingOverrides();
        save_action_map(action_map);            
    }
    private static void save_action_map(InputActionMap action_map){
        string overrides = action_map.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("Rebinds_" + action_map.name, overrides);        
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
