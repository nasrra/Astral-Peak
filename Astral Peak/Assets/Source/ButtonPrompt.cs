using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonPrompt : MonoBehaviour{
    [Header("ButtonPrompt")]
    [SerializeField] protected Image image_icon;
    [SerializeField] protected Transform image_transform;
    [SerializeField] protected string input_action;
    protected InputAction action;
    public void set_input_action(string _input_action) => input_action = _input_action;
    public virtual void initialize(){
        action = InputManager.get_input_action(input_action);
        image_transform.localScale = action.bindings[0].ToDisplayString()=="Space"
            ? new Vector3(image_transform.localScale.x*1.75f,image_transform.localScale.y,image_transform.localScale.z)
            : image_transform.localScale;
        image_icon.sprite = InputManager.get_input_binding_image(action, 0);
    }
    private void uninitialize_wrapper(InputAction.CallbackContext context) => unitialize();
    public virtual void unitialize() => action.performed -= uninitialize_wrapper;
}
