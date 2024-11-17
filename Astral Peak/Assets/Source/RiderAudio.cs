using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderAudio : MonoBehaviour{
    AudioSource source;

    public void emit_arrow_shot()       => AudioClipHandler.play(this, SoundLibrary.get_sound(SoundID.BOW_SHOT),        out source);
    public void emit_magic()            => AudioClipHandler.play(this, SoundLibrary.get_sound(SoundID.MAGIC_1),         out source);
    public void emit_melee_attack()     => AudioClipHandler.play(this, SoundLibrary.get_sound(SoundID.MELEE_SWING_2),   out source);
    public void emit_yell()             => AudioClipHandler.play(this, SoundLibrary.get_sound(SoundID.RIDER_YELL),      out source);
}

