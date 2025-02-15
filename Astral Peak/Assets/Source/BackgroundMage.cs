using UnityEngine;
using System.Collections.Generic;
using Entropek;

public class BackgroundMage : MonoBehaviour{
    [SerializeField] public Animator animator;
    [SerializeField] List<Transform> teleport_points = new List<Transform>();
    [SerializeField] List<Transform> teleport_layers = new List<Transform>();
    [SerializeField] LineParticleEmitter teleport_trail;
    [SerializeField] public BossSpriteHandler sprite_handler;
    [SerializeField] GameObject sprite;
    [SerializeField] GameObject flip_objects;
    public readonly float teleport_time = .5f;
    public void enable_sprite(bool x) => sprite.SetActive(x);
    public void destroy() => Destroy(gameObject, teleport_trail.particles.main.startLifetime.constantMax);
    public void teleport(int point){
        StopAllCoroutines();
        StartCoroutine(Util.timer(
            time:teleport_time,
            start_action:()=>{
                play_teleport_sound();
                sprite_handler.play_death_effect((teleport_time*.75f)-.05f);
            },
            time_out:()=>{
                teleport_trail.emit_once(transform.position, teleport_points[point].position);
                transform.position = teleport_points[point].position;
                transform.parent   = teleport_layers[point];
                sprite_handler.play_death_effect_reverse((teleport_time*.75f)-.05f);
            }
        ));
    }
    void play_teleport_sound(){
        // AudioClipHandler.play(
        // sound_id: Sounds.SoundID.ELECTRIC_BURST,
        // audio_player: gameObject,
        // AudioSourceSettings.NON_DIEGETIC_RANDOMISED);
    }
    public void flip() => flip_objects.transform.rotation = flip_objects.transform.rotation.eulerAngles.y == 0? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
}

