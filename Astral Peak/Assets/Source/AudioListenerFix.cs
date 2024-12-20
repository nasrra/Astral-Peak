using UnityEngine;

// this is done to keep the audio listener from flipping itself:
// causing left to be heard from right and right to be heard from left.

public class AudioListenerFix : MonoBehaviour{
    [SerializeField] Creature player_script;
    [SerializeField] GameObject player_object;

    void handle_flip() => transform.rotation = player_object.transform.rotation.eulerAngles.y == -180? Quaternion.Euler(0,-180,0) : Quaternion.Euler(0, 0,0);

    void Awake(){
        player_script.flipped_left += handle_flip;
        player_script.flipped_right += handle_flip;
    }

    void OnDestroy(){
        player_script.flipped_left -= handle_flip;
        player_script.flipped_right -= handle_flip;
    }
}
