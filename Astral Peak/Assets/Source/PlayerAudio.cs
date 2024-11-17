using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    AudioSource source;

    public void emit_attack()    => AudioClipHandler.play(this, SoundLibrary.get_sound(SoundID.MELEE_SWING_1), out source);
    public void emit_dash()      => AudioClipHandler.play(this, SoundLibrary.get_sound(SoundID.WHOOSH_1), out source);
    public void emit_footsteps() => AudioClipHandler.play(this, choose_footstep(), out source);

    public Sound choose_footstep()
    {
        int x = Random.Range(0, 4);
        switch (x)
        {
            case 0: return SoundLibrary.get_sound(SoundID.SNOW_FOOTSTEP_1);
            case 1: return SoundLibrary.get_sound(SoundID.SNOW_FOOTSTEP_2);
            case 2: return SoundLibrary.get_sound(SoundID.SNOW_FOOTSTEP_3);
            case 3: return SoundLibrary.get_sound(SoundID.SNOW_FOOTSTEP_4);
        }
        throw new System.Exception("ERROR!");
    }
}

