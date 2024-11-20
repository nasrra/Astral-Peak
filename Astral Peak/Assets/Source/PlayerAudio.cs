using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    AudioSource source;

    public void emit_attack()    => AudioClipHandler.play(this, SoundID.MELEE_SWING_1, out source, randomise_pitch: true);
    public void emit_dash()      => AudioClipHandler.play(this, SoundID.WHOOSH_1, out source);
    public void emit_footsteps() => AudioClipHandler.play(this, choose_footstep(), out source, randomise_pitch: true);
    public void emit_attack_hit() => AudioClipHandler.play(this, SoundID.MELEE_HIT, out source, randomise_pitch: true);

    public SoundID choose_footstep()
    {
        int x = Random.Range(0, 4);
        switch (x)
        {
            case 0: return SoundID.SNOW_FOOTSTEP_1;
            case 1: return SoundID.SNOW_FOOTSTEP_2;
            case 2: return SoundID.SNOW_FOOTSTEP_3;
            case 3: return SoundID.SNOW_FOOTSTEP_4;
        }
        throw new System.Exception("ERROR!");
    }
}

