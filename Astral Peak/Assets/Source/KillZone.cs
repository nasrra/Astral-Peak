using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KillZone : MonoBehaviour{
    void OnTriggerEnter2D(Collider2D other){
        Creature creature = other.GetComponent<Creature>();
        if(creature != null)
            creature.kill();
    }
}
