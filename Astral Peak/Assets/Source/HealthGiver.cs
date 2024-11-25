using UnityEngine;

public class HealthGiver : MonoBehaviour{
    [SerializeField] private int amt = 1;
    public void give_player_health(){
        Player.instance.get_health().heal(amt);
    }
}