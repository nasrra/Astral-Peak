using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthGiver : MonoBehaviour{
    [SerializeField] private int amt = 1;
    public void give_player_health(){
        Player.player.get_health().heal(amt);
    }
}