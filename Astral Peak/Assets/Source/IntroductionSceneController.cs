using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroductionSceneController : MonoBehaviour{
    public Animator lore_text, title_card_text;
    [SerializeField] protected List<TextMeshProUGUI> text = new();
    public FireController fire_1, fire_2;
    void Start(){
        List<string> dialogue = ExcelReader.read_file("Dialogue", "IntroductionLore");
        for(int i = 0; i < dialogue.Count; i++)
            text[i].text = dialogue[i];
        CutsceneManager.play(new Introduction(this));
    }
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
        AudioManager.play_music("music_introduction");
        yield return new WaitForSeconds(14.25f);
        controller.fire_1.turn_on();
        yield return new WaitForSeconds(4);
        controller.fire_2.turn_on();
        yield return new WaitForSeconds(4);
        controller.lore_text.Play("fade_in");
        yield return new WaitForSeconds(40);
        controller.fire_1.turn_off();
        yield return new WaitForSeconds(4);
        controller.fire_2.turn_off();
        yield return new WaitForSeconds(4);
        AudioManager.stop_music();
        stop_skip();
        yield return new WaitForSeconds(2);
        CustomSceneManager.load_scene_with_transitions("TutorialRoom1");
        end();
        yield break;
    }
}