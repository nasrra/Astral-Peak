using UnityEngine;
using UnityEngine.TextCore.Text;

public class DomineDoor : MonoBehaviour{
    [SerializeField] Animator animator;
    [SerializeField] Collider2D cutscene_trigger;
    [SerializeField] Transform player_point;
    [SerializeField] bool start_open = false;

    void Awake(){
        if(start_open == true)
            opened();
        else
            closed();
    }

    public void open(){
        animator.Play("open");
        play_sound();
        shake_camera();
    }
    public void opened() => animator.Play("opened");
    public void close(){
        animator.Play("close");
        play_sound();
        shake_camera();
    }
    public void closed() => animator.Play("closed");

    void OnTriggerEnter2D(Collider2D other){
        cutscene_trigger.enabled = false;
        CutsceneManager.play(new Cutscenes.DomineDoorFinal());
    }

    private void just_opened() => cutscene_trigger.enabled = true;
    private void just_close() => cutscene_trigger.enabled = false;

    public Transform get_player_point() => player_point;

    private void play_sound(){
        // AudioClipHandler.play(
        // Sounds.SoundID.STONE_DOOR,
        // gameObject,
        // AudioSourceSettings.NON_DIEGETIC_RANDOMISED
        // );
    }
    private void shake_camera() => CameraController.instance.shake_camera(5f,.75f,true);

}
