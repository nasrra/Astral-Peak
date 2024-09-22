using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour{
    [SerializeField] private PlayerInput input;
    private Keybinds keybinds;
    public void Start(){
        // enable keyboard keybinds
        keybinds = new Keybinds();
        keybinds.Keyboard.Enable();
        keybinds.Keyboard.Jump.performed += Jump;
        keybinds.Keyboard.Left.performed += Left;
        keybinds.Keyboard.Right.performed += Right;
    }

    public void Jump(InputAction.CallbackContext ctx){
        Debug.Log("jump");
    }

    public void Left(InputAction.CallbackContext ctx){
        Debug.Log("left");
    }

    public void Right(InputAction.CallbackContext ctx){
        Debug.Log("right");
    }
}
