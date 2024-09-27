using UnityEngine;

// this class is to enable the changing of animation parameters during an animation.
// dont know why unity doesnt have this built in, but it is what it is.

public class AnimationExtension : MonoBehaviour{
    [SerializeField] private Animator animator;
    public void SetTrigger(string trigger_name) => animator.SetTrigger(trigger_name);
    public void ResetTrigger(string trigger_name) => animator.ResetTrigger(trigger_name);
}
