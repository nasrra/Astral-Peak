using DocumentFormat.OpenXml.Presentation;
using UnityEngine;

public class MagicSummonProjectile : TrackingProjectile{
    AudioSource source;
    void Awake(){
        state_switch(ref rotate_state, rotate_to_target(Player.instance.transform, snap_rotate_speed));
        buffer();
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
        enable_colliders(false);
    }


}
