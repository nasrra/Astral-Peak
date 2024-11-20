using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// this is a class for static components that are used everywhere within the game.
// The [RuntimeInitializeOnLoadMethod] attribute will ensure InitializeOnStart is called as soon as the game starts.
// The RuntimeInitializeLoadType.BeforeSceneLoad ensures the method runs before any scene loads, so it can be used to set up essential components at the start.

public static class StaticComponents{
    public static GameObject main;
    static UnityHook hook_in;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    // initializing the managers of the game.
    static void initialize(){
        main = GameObject.Instantiate(new GameObject());
        main.name = "Managers";
        main.AddComponent<DontDestroyOnLoad>();
        hook();
        input();
        audio();
        cutscene();
        Application.quitting += uninitialize;
    }

    static void uninitialize(){
        SoundLibrary.uninitialize();
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
        List<AudioSource> sources = new List<AudioSource>();
        for(int i = 0; i < 2; ++i){
            AudioSource source = main.AddComponent<AudioSource>();
            source.volume = 0;
            sources.Add(source);
        }
        AudioManager.initialize(sources, hook_in);
        SoundLibrary.initialize();
    }

    // cutscene manager.
    static void cutscene() => CutsceneManager.initialize(hook_in);

    // hook into unity engines runtime.
    static void hook(){
        hook_in = main.AddComponent<UnityHook>();
        hook_in.start += AudioManager.on_start; // this only works when on start is called.
    }
}
