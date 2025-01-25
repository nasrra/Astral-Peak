using UnityEngine;

public class TeleportParticleEmitter : LineParticleEmitter{
    void Awake() => emitted = () => AudioClipHandler.play(Sounds.SoundID.THUNDER_1, this, AudioSourceSettings.NON_DIEGETIC_RANDOMISED);
}
