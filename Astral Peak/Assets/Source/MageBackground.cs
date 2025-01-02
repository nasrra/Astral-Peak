using UnityEngine;
using System.Collections.Generic;

public class MageBackground : MonoBehaviour{
    [SerializeField] List<Transform> teleport_points = new List<Transform>();
    [SerializeField] List<Transform> teleport_layers = new List<Transform>();
    [SerializeField] LineParticleEmitter teleport_trail;
    [SerializeField] GameObject sprite;
    public void teleport_to_point(int point){
        teleport_trail.emit_once(transform.position, teleport_points[point].position);
        transform.position = teleport_points[point].position;
        transform.rotation = teleport_points[point].rotation;       
        transform.parent = teleport_layers[point]; 
    }
    public void enable_sprite(bool x) => sprite.SetActive(x);
    public void destroy() => Destroy(gameObject, teleport_trail.particles.main.startLifetime.constantMax);
    void play_sound() =>
        AudioClipHandler.play(
        sound_id: Sounds.SoundID.ELECTRIC_BURST,
        audio_player: this,
        AudioSourceSettings.NON_DIEGETIC_RANDOMISED
    );
}
