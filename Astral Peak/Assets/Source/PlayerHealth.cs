using UnityEngine;
using System.Collections;

public class PlayerHealth : Health{
    float regen_time = 10;
    Coroutine regen_state;

    void Awake() => link();
    void OnDestroy() => unlink();

    void link() => damaged += start_regen_timer;
    void unlink() => damaged -= start_regen_timer;

    void start_regen_timer() => state_switch(ref regen_state, regen_timer());

    IEnumerator regen_timer(){
        yield return new WaitForSeconds(regen_time);        
        heal(1); 
        if(data.current_life < data.max_life) // regen further if there is still more life to heal.
            start_regen_timer();
        yield break;
    }
}
