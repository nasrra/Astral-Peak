using Entropek;
using UnityEngine;

public class CutsceneUI : MonoBehaviour{
    [SerializeField] Animator skip_button;
    Coroutine skip_button_state;
    void Awake(){
        link();
        check_game_state();
    }
    
    void OnDestroy(){
        unlink();
    }

    void start_skip(){
        skip_button_state = StartCoroutine(Util.timer(
            CutsceneManager.skip_buffer_time,
            start_action: ()=>{
                skip_button.Play("held",0,0);
                //skip_button.playbackTime = 1/CutsceneManager.skip_buffer_time;
            },
            time_out:()=>unlink_cutscene_skip()
        ));
    }

    void stop_skip(){
        if(skip_button_state != null)
            StopCoroutine(skip_button_state);
        skip_button.Play("on",0,0);
    }


    void on(){
        skip_button.Play("on",0,0);
        link_cutscene_skip();
    }

    void fade_in(){
        skip_button.Play("fade_in",0,0);
        link_cutscene_skip();
    }

    void fade_out(){
        skip_button.Play("fade_out",0,0);
        unlink_cutscene_skip();
    }

    void check_game_state(){
        if(GameManager.get_state() == GameState.CUTSCENE)
            on();
    }

    void entered_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            fade_in();
    }

    void exited_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            fade_out();
    }




    // linkage

    void link_cutscene_skip(){
        InputManager.cutscene_skip_performed += start_skip;
        InputManager.cutscene_skip_canceled  += stop_skip;
    }

    void unlink_cutscene_skip(){
        InputManager.cutscene_skip_performed -= start_skip;
        InputManager.cutscene_skip_canceled  -= stop_skip;        
    }

    void link(){
        GameManager.entered_game_state  += entered_game_state;
        GameManager.exited_game_state   += exited_game_state;
    }

    void unlink(){
        unlink_cutscene_skip();
        GameManager.entered_game_state  -= entered_game_state;
        GameManager.exited_game_state   -= exited_game_state;
    }
}
