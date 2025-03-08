using System.Collections;
using UnityEngine;

public class Beatrice : MonoBehaviour{
    [SerializeField] AudioPlayer audio_player;
    [SerializeField] Animator animator;
    [SerializeField] SpriteHandler sprite;
    [SerializeField] ParticleHandler particles;

    void Awake(){
        idle();
    } 

    public void idle(){
        animator.Play("idle",0);
        animator.Play("idle",1);
    }

    public void awaken()=>StartCoroutine(awaken_coroutine());

    IEnumerator awaken_coroutine(){
        audio_player.play_non_diegetic_one_shot("heart_thump");
        StartCoroutine(sprite.lerp_value("_charge_amount",0, .15f, 1f));
        particles.play_particle("awaken_ambience");
        animator.Play("awaken",1);
        StartCoroutine(sprite.lerp_value("_charge_amount",.15f, 0, 1f));
        yield break;
    }
}
