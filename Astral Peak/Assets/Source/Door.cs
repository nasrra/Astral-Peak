using System;
using UnityEngine;

public abstract class Door : MonoBehaviour{





    // variables
    public event Action now_opened, now_closed;
    [Header("Door")]
    [SerializeField] SpawnPoint spawn_point;
    [SerializeField] Animator animator;
    [SerializeField] bool start_open = false;
    [SerializeField] Collider2D solid_col, trigger_col;





    // Base.
    void Start(){
        if(start_open == true)
            opened();
        else
            closed();
        link();
    }
    void OnDisable() => unlink();
    void OnTriggerEnter2D() => enter();






    // states:
    public void set_start_open(bool open) => start_open = open;
    public abstract void enter();
    void exit(){
        opened();
        close();
    }
    public void open(){
        if(animator != null)
            animator?.Play("open");
    }
    public void opened(){
        if(animator != null)
            animator.Play("opened");
        set_trigger();
    }
    public void finished_openening() => now_opened?.Invoke();
    public void close(){
        if(animator != null)
            animator.Play("close");
    }
    public void closed(){
        if(animator != null)
            animator.Play("closed");
        set_solid();
    }
    public void finished_closing() => now_closed?.Invoke();
    void set_trigger(){
        if(trigger_col != null)
            trigger_col.enabled = true;
        if(solid_col != null)
            solid_col.enabled = false;
    }
    void set_solid(){
        if(trigger_col != null) 
            trigger_col.enabled = false;
        if(solid_col != null)
            solid_col.enabled = true;
    }





    // Linkage.
    void link(){
        now_closed += set_solid;
        now_opened += set_trigger;
        if(spawn_point != null)
            spawn_point.spawn_used += exit;
    }
    void unlink(){
        now_closed -= set_solid;
        now_opened -= set_trigger;        
        if(spawn_point != null)
            spawn_point.spawn_used += exit;
    }
}
