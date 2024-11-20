using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    AudioSource source;

    public void emit_attack()    => AudioClipHandler.play(this, SoundID.MELEE_SWING_1, out source);
    public void emit_dash()      => AudioClipHandler.play(this, SoundID.WHOOSH_1, out source);
    public void emit_footsteps() => AudioClipHandler.play(this, choose_footstep(), out source);
    public void emit_attack_hit() => AudioClipHandler.play(this, choose_attack_hit(), out source);

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


    public SoundID choose_attack_hit()
    {
        int x = Random.Range(0, 3);
        switch (x)
        {
            case 0: return SoundID.MELEE_HIT_1;
            case 1: return SoundID.MELEE_HIT_2;
            case 2: return SoundID.MELEE_HIT_3;
        }
        throw new System.Exception("ERROR!");
    }
}

