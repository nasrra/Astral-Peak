using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavalryCutsceneArrow : MonoBehaviour{
    [SerializeField] SmartTurret turret;
    AudioSource source;
    public void shoot() => turret.fire_once();
    void play_arrow_shot(GameObject x) => AudioClipHandler.play(this, SoundID.BOW_SHOT, out source); 
    void Start() => turret.projectile_fired += play_arrow_shot;
    void OnDestroy() => turret.projectile_fired += play_arrow_shot;
}
