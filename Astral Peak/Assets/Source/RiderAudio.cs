using UnityEngine;

public class RiderAudio : MonoBehaviour{
    AudioSource source;

    public void emit_whistle()          => AudioClipHandler.play(this, SoundID.WHISTLE_LONG,    out source);
    public void emit_arrow_shot()       => AudioClipHandler.play(this, SoundID.BOW_SHOT,        out source);
    public void emit_magic()            => AudioClipHandler.play(this, SoundID.MAGIC_1,         out source);
    public void emit_melee_attack()     => AudioClipHandler.play(this, SoundID.MELEE_SWING_2,   out source);
    public void emit_yell()             => AudioClipHandler.play(this, SoundID.RIDER_YELL,      out source);
}

