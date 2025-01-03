using UnityEngine;
using System.Collections.Generic;

public class MageBackground : MonoBehaviour{
    [SerializeField] List<Transform> teleport_points = new List<Transform>();
    [SerializeField] List<Transform> teleport_layers = new List<Transform>();
    [SerializeField] LineParticleEmitter teleport_trail;
    [SerializeField] BossSpriteHandler sprite_handler;
    [SerializeField] GameObject sprite;
    public void teleport_to_point(int point){
        teleport_trail.emit_once(transform.position, teleport_points[point].position);
        transform.position = teleport_points[point].position;
        transform.rotation = teleport_points[point].rotation;       
        transform.parent = teleport_layers[point]; 
    }
    public void enable_sprite(bool x) => sprite.SetActive(x);
    public void destroy() => Destroy(gameObject, teleport_trail.particles.main.startLifetime.constantMax);
    public void teleport_enter() => sprite_handler.play_death_effect(.2f);
    public void teleport_exit() => sprite_handler.play_death_effect_reverse(.2f);

}

