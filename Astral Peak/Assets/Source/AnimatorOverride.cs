using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimatorOverride : MonoBehaviour{
    [SerializeField] protected Animator animator;
    public void Play(int animation_id) => animator.Play(animation_id);
    public void Play(string v) => animator.Play(v);
}
