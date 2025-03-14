using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DomineDoor : MonoBehaviour{
    [SerializeField] Animator animator;
    [SerializeField] AudioPlayer audio_player;
    [SerializeField] Collider2D cutscene_trigger;
    [SerializeField] Transform player_point;
    [SerializeField] ParticleHandler particles;
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
        shake_camera();
        audio_player.play_non_diegetic_one_shot("domine_door_non_diegetic");
        particles.play_particle("transition_ambience_left");
        particles.play_particle("transition_ambience_right");
    }
    public void opened() => animator.Play("opened");
    public void close(){
        animator.Play("close");
        shake_camera();
        audio_player.play_non_diegetic_one_shot("domine_door_non_diegetic");
        particles.play_particle("transition_ambience_left");
        particles.play_particle("transition_ambience_right");
    }
    public void closed() => animator.Play("closed");

    void OnTriggerEnter2D(Collider2D other){
        cutscene_trigger.enabled = false;
        CharacterMovement player_movement = Player.instance.get_movement() as CharacterMovement;
        Player.instance.transform.position = player_point.position;
        Player.instance.transform.parent = player_point;
        if(final_cutscene == true)
            CutsceneManager.play(new Cutscenes.DomineDoorFinal());
        else{
            StartCoroutine(enter_game_end_stinger());
            Player.instance.lock_player();
        }
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
        yield return new WaitForSeconds(8);
        CustomSceneManager.load_scene_with_transitions("GameEndStinger");
        yield break;
    }

    private void just_opened(){
        audio_player.play_diegetic_one_shot("stone_door_shut");
        cutscene_trigger.enabled = true;
        particles.stop_all_particles();
        particles.get_particle("transition_ambience_left").Emit(15);
        particles.get_particle("transition_ambience_right").Emit(15);
    }
    private void just_closed(){
        audio_player.play_diegetic_one_shot("stone_door_shut");
        cutscene_trigger.enabled = false;
        particles.stop_all_particles();
        particles.get_particle("transition_ambience_left").Emit(15);
        particles.get_particle("transition_ambience_right").Emit(15);
    }

    public Transform get_player_point() => player_point;
    private void shake_camera() => CameraController.instance.shake_camera(4f,.5f,true);

}
