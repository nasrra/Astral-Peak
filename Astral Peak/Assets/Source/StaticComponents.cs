using System.Text;
using Entropek;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// this is a class for static components that are used everywhere within the game.
// The [RuntimeInitializeOnLoadMethod] attribute will ensure InitializeOnStart is called as soon as the game starts.
// The RuntimeInitializeLoadType.BeforeSceneLoad ensures the method runs before any scene loads, so it can be used to set up essential components at the start.


public static class StaticComponents{
    public static GameObject main;
    static UnityHook hook_in;

    

    static GameObject player_input;

    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    // initializing the managers of the game.
    static void initialize(){
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        // Call this once during application initialization
        main = GameObject.Instantiate(new GameObject());
        main.name = "Managers";
        GameObject.DontDestroyOnLoad(main);
        hook();
        input();
        audio();
        cutscene();
        scene_manager();
        file_manager();
        game_manager();
        display_manager();
        Application.quitting += uninitialize;
    }

    static void uninitialize(){
        // SoundLibrary.uninitialize();
        InputManager.uninitialize();
        GameManager.uninitialize();
        CustomSceneManager.uninitialize();
        PlayerPrefs.Save();
    }

    // input initialization.
    static void input(){
        PlayerInput input = main.AddComponent<PlayerInput>();
        InputActionAsset action_asset = Resources.Load<InputActionAsset>("Action Assets/Keybinds");
        input.actions = action_asset;
        input.defaultActionMap = "Keyboard";
        input.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        InputManager.initialize(input);
    }

    // initialize audio sources.
    static void audio(){
        // AudioManager.initialize();
        // SoundLibrary.initialize();
    }

    // cutscene manager.
    static void cutscene() => CutsceneManager.initialize(hook_in);

    // hook into unity engines runtime.
    static void hook(){
        hook_in = main.AddComponent<UnityHook>();
        // hook_in.start += AudioManager.on_start; // this only works when on start is called.
    }

    static void scene_manager(){
        SceneManager.activeSceneChanged += scene_changed;
        CustomSceneManager.initialize();
    }
    static void scene_changed(Scene scene_1, Scene scene_2){
        //AudioManager.restore_sfx_smooth();
        //InputManager.reset_input_blockers(); // reset input blockers so the player cant mess up move direction when holding down keys.
    }

    static void file_manager(){
        FileManager.set_default_file_path(Application.persistentDataPath, "SaveFile.json");
    }

    static void game_manager() => GameManager.initialize();
    static void display_manager() => DisplaySettingsManager.initialize();
}
