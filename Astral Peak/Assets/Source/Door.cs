using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class Door : MonoBehaviour{





    // Data.
    public event Action now_opened, now_closed;
    [SerializeField] Animator animator = null;
    [SerializeField] private string scene_to_load, exit_point;
    [SerializeField] Collider2D trigger_col, solid_col;
    [SerializeField] bool start_open = false;
    [SerializeField] SpawnPoint spawn_point;





    // Base.
    void Awake(){
        if(start_open == true)
            opened();
        link();
    }
    void OnDisable(){
        unlink();
    }
    void OnTriggerEnter2D(){
        if(scene_to_load != "")
            enter();
    }






    // States.
    public virtual void enter() => StartCoroutine(enter_coroutine());
    IEnumerator enter_coroutine(){
        Player.instance.set_spawn_point(exit_point);
        Player.instance.door_enter_state();
        CustomSceneManager.load_scene(scene_to_load); 
        close();
        yield break;
    }
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