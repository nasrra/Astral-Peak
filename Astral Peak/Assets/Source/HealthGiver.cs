using UnityEngine;

public class HealthGiver : MonoBehaviour{
    [SerializeField] private int amt = 1;
    public void give_player_health(){
        Player.player.get_health().heal(amt);
    }
}