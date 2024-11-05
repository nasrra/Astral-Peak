using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour{
    [SerializeField] private string 
        enter_point, scene_to_load, exit_point;

    void Awake() => DoorManager.add_door(this);
    void OnDisable() => DoorManager.erase_door(this);


    public virtual void enter(){
        Player.exit_point = exit_point;
        SceneManager.LoadScene(scene_to_load);
    }
    public void set_enter_point(string new_enter_point) => enter_point = new_enter_point;
    public void set_exit_point(string new_exit_point) => exit_point = new_exit_point;
    public void set_scene_to_load(string new_scene_to_load) => scene_to_load = new_scene_to_load;
    public string get_enter_point() => enter_point;
    public string get_exit_point() => exit_point;
    public string get_scene_to_load() => scene_to_load;
}