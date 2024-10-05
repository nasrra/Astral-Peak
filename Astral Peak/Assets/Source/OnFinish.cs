using UnityEngine;

public class OnFinish : StateMachineBehaviour{
    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public string next_animation;
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        // Check if the animation is at its final frame (normalizedTime = 1 means it's finished)
        if (stateInfo.normalizedTime >= 1.0f)  
            animator.Play(next_animation);
    }
}
