using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;

public class Door : MonoBehaviour{
    public event Action now_opened, now_closed;
    [SerializeField] Animator animator;
    [SerializeField] private string scene_to_load, exit_point;
    [SerializeField] Transform enter_point;
    [SerializeField] Collider2D trigger_col, solid_col;
    [SerializeField] bool start_open = false;

    void Awake(){
        DoorManager.add_door(this);
        if(start_open == true)
            open();
        link();
    }
    void OnDisable(){
        DoorManager.erase_door(this);
        unlink();
    }


    void OnTriggerEnter2D() => enter();

    public virtual void enter() => StartCoroutine(enter_coroutine());
    IEnumerator enter_coroutine(){
        Player.instance.set_exit_point(exit_point);
        Player.instance.door_enter_state();
        AudioManager.dim_sfx_smooth();
        UiManager.instance.fade_to_black();
        yield return new WaitForSeconds(2f);

        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = activeScene.GetRootGameObjects();
        // Destroy each GameObject so that their unlink functions are correctle called :)
        foreach (GameObject obj in rootObjects){
            Destroy(obj);
        }
        SceneManager.LoadScene(scene_to_load);        
    }
    public void set_exit_point(string new_exit_point) => exit_point = new_exit_point;
    public void set_scene_to_load(string new_scene_to_load) => scene_to_load = new_scene_to_load;
    public Transform get_enter_point() => enter_point;
    public string get_exit_point() => exit_point;
    public string get_scene_to_load() => scene_to_load;
    
    public void open() => animator.Play("open");
    public void close() => animator.Play("close");
    public void finished_openening() => now_opened?.Invoke();
    public void finished_closing() => now_closed?.Invoke();
    public MovementOption get_exit_direction() => (enter_point.position - transform.position).x > 0? MovementOption.RIGHT : MovementOption.LEFT; 

    void set_trigger(){
        trigger_col.enabled = true;
        solid_col.enabled = false;
    }
    void set_solid(){
        trigger_col.enabled = false;
        solid_col.enabled = true;
    }

    void link(){
        now_closed += set_solid;
        now_opened += set_trigger;
    }

    void unlink(){
        now_closed -= set_solid;
        now_opened -= set_trigger;        
    }
}