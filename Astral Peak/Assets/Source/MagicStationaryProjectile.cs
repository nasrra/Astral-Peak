using Entropek;
using UnityEngine;

public class MagicStationaryProjectile : Projectile{
    [Header("Magic Stationary Projectile")]
    [SerializeField] ParticleSystem ambience;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] protected TrailRenderer trail;
    
    protected override void play_sound(){
        audio_player.play_diegetic_one_shot("electricity_burst_soft");
        audio_player.play_diegetic_loop("electricity_crackle_soft");
    }
    protected override void stop_sound(){
        audio_player.stop_loop("electricity_crackle_soft");  
    }

    void OnEnable(){
        play_sound();
    }

    void OnDisable(){
        stop_sound();    
    }

    public override void destroy(){
        ambience.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        base.destroy();
    }
}
