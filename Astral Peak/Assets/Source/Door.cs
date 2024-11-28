using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour{
    [SerializeField] private string scene_to_load, exit_point;
    [SerializeField] Transform enter_point;
    [SerializeField] DoorAnimator animator;
    [SerializeField] Collider2D trigger_col, solid_col;
    

    void Awake(){
        DoorManager.add_door(this);
        link();
    }
    void OnDisable(){
        DoorManager.erase_door(this);
        unlink();
    }


    void OnTriggerEnter2D() => enter();

    public virtual void enter(){
        Player.instance.set_exit_point(exit_point);

        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = activeScene.GetRootGameObjects();
        // Destroy each GameObject so that their unlink functions are correctle called :)
        foreach (GameObject obj in rootObjects){
            if(obj != this)
                Destroy(obj);
        }
        SceneManager.LoadScene(scene_to_load);
    }
    public void set_exit_point(string new_exit_point) => exit_point = new_exit_point;
    public void set_scene_to_load(string new_scene_to_load) => scene_to_load = new_scene_to_load;
    public Transform get_enter_point() => enter_point;
    public string get_exit_point() => exit_point;
    public string get_scene_to_load() => scene_to_load;
    
    void set_trigger(){
        trigger_col.enabled = true;
        solid_col.enabled = false;
    }
    void set_solid(){
        trigger_col.enabled = false;
        solid_col.enabled = true;
    }

    void link(){
        animator.now_closed += set_solid;
        animator.now_opened += set_trigger;
    }

    void unlink(){
        animator.now_closed -= set_solid;
        animator.now_opened -= set_trigger;        
    }
}