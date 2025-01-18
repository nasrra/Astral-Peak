using Entropek;
using UnityEngine;

public class NewGameButton : MonoBehaviour{
    public void invoke(){
        FileManager.delete_data(); // delete the previous save file.
        CustomSceneManager.load_scene_with_transitions("Shrine");
    }
}
