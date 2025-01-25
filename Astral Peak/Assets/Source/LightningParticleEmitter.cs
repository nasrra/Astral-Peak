using UnityEngine;

public class LightningParticleEmitter : LineParticleEmitter{
    void Awake() => emitted = () => AudioClipHandler.play(Sounds.SoundID.THUNDER_1, this, AudioSourceSettings.DIEGETIC_RANDOMISED_LOOP);
}
