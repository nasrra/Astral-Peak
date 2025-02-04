using System.Collections;
using Entropek;
using UnityEngine;

public class FinalBossRoomGroundHandler : MonoBehaviour{
    public Rigidbody2D[] grounds; // Array to hold all the squares

    void Start(){
        StartCoroutine(test());
    }

    // Method to add a new wave at a specific index
    public void start_wave(int index, bool move_left, float rate, float force){
        int count = 0;
        if(index >= grounds.Length)
            return;
        if(move_left == true){
            for(int i = index; i >= 0; i--){
                int selected = i;
                StartCoroutine(Util.timer(
                    count * rate,
                    time_out:()=>grounds[selected].AddForce(new Vector2(0,force), ForceMode2D.Impulse)
                ));
                count++;
            }
        }
        else
            for(int i = index; i < grounds.Length; i++){
                int selected = i;
                StartCoroutine(Util.timer(
                    count * rate,
                    time_out:()=>grounds[selected].AddForce(new Vector2(0,force), ForceMode2D.Impulse)
                ));
                count++;
            }
    }

    IEnumerator test(){
        while(true){
            yield return new WaitForSeconds(4);
            start_wave(7, true, .1f, 400);
            yield return new WaitForSeconds(4);
            start_wave(8, false, .1f, 400);
            yield return null;
        }
    }
}

