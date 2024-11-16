using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderAudio : MonoBehaviour{
    Sound
        yell,
        arrow_shot,
        melee_attack,
        magic;
    AudioSource source;
    public void Awake(){
        yell = SoundLibrary.sfx["rider_yell"]();
        arrow_shot = SoundLibrary.sfx["bow_shot"]();
        melee_attack = SoundLibrary.sfx["melee_swing_2"]();
        magic = SoundLibrary.sfx["magic_1"]();
    }
    public void emit_yell() => AudioClipHandler.play(this, yell, out source);
    public void emit_arrow_shot() => AudioClipHandler.play(this, arrow_shot, out source);
    public void emit_melee_attack() => AudioClipHandler.play(this, melee_attack, out source);
    public void emit_magic()        => AudioClipHandler.play(this, magic, out source);
}
