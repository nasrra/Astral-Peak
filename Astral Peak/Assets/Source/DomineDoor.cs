using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DomineDoor : MonoBehaviour{
    [SerializeField] Animator animator;
    [SerializeField] Collider2D cutscene_trigger;
    [SerializeField] Transform player_point;
    [SerializeField] bool start_open = false;
    [SerializeField] bool final_cutscene = true;

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
        CharacterMovement player_movement = Player.instance.get_movement() as CharacterMovement;
        if(final_cutscene == true)
            CutsceneManager.play(new Cutscenes.DomineDoorFinal());
        else
            StartCoroutine(enter_game_end_stinger());
    }

    IEnumerator enter_game_end_stinger(){
        InputManager.disable_user_input();
        CharacterMovement player_movement = Player.instance.get_movement() as CharacterMovement;
        Player.instance.transform.position = player_point.position;
        Player.instance.StopAllCoroutines();
        // player_movement.mod_gravity(0);
        // player_movement.halt();
        Player.instance.animator.force_idle();
        Player.instance.sprite.fade_to_black();
        Player.instance.sprite.enter_domine_door_layer();
        yield return new WaitForSeconds(4);
        CustomSceneManager.load_scene_with_transitions("GameEndStinger");
        yield break;
    }

    private void just_opened() => cutscene_trigger.enabled = true;
    private void just_close() => cutscene_trigger.enabled = false;

    public Transform get_player_point() => player_point;

    private void play_sound(){
        AudioManager.play_non_diegetic_one_shot("domine_door_non_diegetic");
    }
    private void shake_camera() => CameraController.instance.shake_camera(4f,.5f,true);

}
