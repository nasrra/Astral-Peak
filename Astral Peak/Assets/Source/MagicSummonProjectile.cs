using UnityEngine;

public class MagicSummonProjectile : TrackingProjectile{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        state_switch(ref rotate_state, rotate_to_target(Player.instance.transform, snap_rotate_speed));
        StartCoroutine(buffer_timer());
        AudioClipHandler.play(
            Sounds.SoundID.ELECTRICITY_3,
            this,
            AudioSourceSettings.DIEGETIC_RANDOMISED_LOOP
        );
    }
}
