using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour{
    AudioSource source_1, source_2;
    void Start() => StartCoroutine(test());
    protected IEnumerator test(){
        while (true){
            AudioManager.play_music(Sounds.SoundID.WOLF_BOSS_MUSIC_1);
            yield return new WaitForSeconds(5);
            AudioManager.play_music(Sounds.SoundID.WOLF_BOSS_MUSIC_2);
            yield return new WaitForSeconds(7);
            AudioManager.stop_music();
            yield return new WaitForSeconds(5);
        }
    }
}