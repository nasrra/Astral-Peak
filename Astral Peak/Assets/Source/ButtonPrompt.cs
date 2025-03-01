using Entropek;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonPrompt : MonoBehaviour{
    [Header("ButtonPrompt")]
    [SerializeField] protected Image image_icon;
    [SerializeField] protected Transform image_transform;
    [SerializeField] protected string input_action;
    [HideInInspector] public InputAction action;
    public void set_input_action(string _input_action) => input_action = _input_action;
    public virtual void initialize(){
        action = InputManager.get_input_action(input_action);
        image_transform.localScale = action.bindings[0].ToDisplayString()=="Space"
            ? new Vector3(1.5f,1.5f,1)
            : new Vector3(1,1,1);
        image_icon.sprite = InputManager.get_input_binding_image(action, 0);
    }
    public virtual void unitialize(){
        image_transform.localScale = new Vector3(1,1,1);
    }
}
