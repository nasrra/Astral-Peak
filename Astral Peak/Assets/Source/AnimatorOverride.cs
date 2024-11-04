using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimatorOverride : MonoBehaviour{
    [SerializeField] protected Animator animator;
    public abstract void idle();
    public abstract void run();
    public void Play(int animation_id) => animator.Play(animation_id);
}
