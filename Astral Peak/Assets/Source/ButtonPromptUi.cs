using UnityEngine;

public class ButtonPromptUi : ButtonPrompt{
    void OnEnable()=>initialize();
    void OnDisable()=>unitialize();
    public void rebind() => InputManager.rebind_action(action, ()=>{unitialize(); initialize();});
}
