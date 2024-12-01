using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour{
    public void load_game() => SceneManager.LoadScene("Shrine");
}
