using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Remember to uncheck "Write Defaults" in the animator controller to avoid any funky business.
// may also need to start adding states that go for one frame that "Write's Defaults" for a animator then goes to the idle state.

public abstract class AnimatorOverride : MonoBehaviour{
    [SerializeField] protected Animator animator;
    public void Play(int animation_id) => animator.Play(animation_id);
    public void Play(string v) => animator.Play(v);
}
