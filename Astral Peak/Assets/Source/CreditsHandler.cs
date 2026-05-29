using System;
using UnityEngine;

public class CreditsHandler : MonoBehaviour{
    public event Action credits_ended;
    [SerializeField] Animator animator;

    public void start_credits(){
        animator.Play("loop");
    }

    public void end_credits(){
        credits_ended?.Invoke();
    }
}
