using System.Collections.Generic;
using UnityEngine;

public class SummoningCircleHandler : MonoBehaviour{
    [SerializeField] List<Animator> summoning_circles = new List<Animator>();
    [SerializeField] AudioPlayer audio_player;
    public void turn_on(){
        audio_player.play_non_diegetic_one_shot("deep_gong");
        foreach(Animator circle in summoning_circles)
            circle.Play("turn_on");
    }
    public void turn_on(int start, int end){
        audio_player.play_non_diegetic_one_shot("deep_gong");
        for(int i = start; i < end+1; i++)
            summoning_circles[i].Play("turn_on");
    }
    public void turn_on(List<int> circles){
        audio_player.play_non_diegetic_one_shot("deep_gong");
        foreach(int i in circles)
            summoning_circles[i].Play("turn_on");
    }
    public void turn_off(){
        foreach(Animator circle in summoning_circles)
            circle.Play("turn_off");
    }
    public void turn_off(int start, int end){
        for(int i = start; i < end+1; i++)
            summoning_circles[i].Play("turn_on");
    }
    public void turn_off(List<int> circles){
        foreach(int i in circles)
            summoning_circles[i].Play("turn_off");
    }
    public void off(){
        foreach(Animator animator in summoning_circles)
            animator.Play("off");        
    }
    public void on(){
        foreach(Animator animator in summoning_circles)
            animator.Play("on");        
    }
}
