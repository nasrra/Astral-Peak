using System.Collections;
using UnityEngine;

public class IntroductionSceneController : MonoBehaviour{
    public Animator lore_text, title_card_text;
    public FireController fire_1, fire_2;
    void Start() => CutsceneManager.play(new Introduction(this));
}

public class Introduction : Cutscene{
    IntroductionSceneController controller;
    public Introduction(IntroductionSceneController controller){
        this.controller = controller;
    }
    public override IEnumerator get_coroutine(){ 
        controller.fire_1.off();
        controller.fire_2.off();
        yield return new WaitForSeconds(1);
        controller.title_card_text.Play("fade_in");
        AudioManager.play_music(Sounds.SoundID.INTRODUCTION_MUSIC);
        yield return new WaitForSeconds(10);
        controller.fire_1.turn_on();
        yield return new WaitForSeconds(4);
        controller.fire_2.turn_on();
        yield return new WaitForSeconds(6);
        controller.lore_text.Play("fade_in");
        yield return new WaitForSeconds(62);
        controller.fire_1.turn_off();
        yield return new WaitForSeconds(4);
        controller.fire_2.turn_off();
        yield return new WaitForSeconds(4);
        AudioManager.stop_music();
        yield return new WaitForSeconds(2);
        CustomSceneManager.load_scene_with_transitions("Shrine");
        end();
        yield break;
    }
}