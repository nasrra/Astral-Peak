using DocumentFormat.OpenXml.Presentation;
using UnityEngine;

public class MagicSummonProjectile : TrackingProjectile{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    AudioSource source;
    void Start(){
        state_switch(ref rotate_state, rotate_to_target(Player.instance.transform, snap_rotate_speed));
        StartCoroutine(buffer_timer());
        source = AudioClipHandler.play(
            Sounds.SoundID.ELECTRICITY_3,
            this,
            AudioSourceSettings.DIEGETIC_RANDOMISED_LOOP
        );
        death_started += fade_out;
    }

    void fade_out(){
        AudioClipHandler.fade_out(
            this,
            source,
            deathtime/4
        );
        death_started -= fade_out;
        enable_collider(false);
    }


}
