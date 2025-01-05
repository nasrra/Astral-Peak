using System.Collections.Generic;
using UnityEngine;

public class SummoningCircleHandler : MonoBehaviour{
    [SerializeField] List<Animator> summoning_circles = new List<Animator>();
    public void turn_on(){
        foreach(Animator circle in summoning_circles)
            circle.Play("turn_on");
    }
    public void turn_on(int start, int end){
        for(int i = start; i < end+1; i++)
            summoning_circles[i].Play("turn_on");
    }
    public void turn_on(List<int> circles){
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
}
