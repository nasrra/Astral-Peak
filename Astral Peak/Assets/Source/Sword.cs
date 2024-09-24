using UnityEngine;

public class Sword : Weapon{
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Enemy")
            Debug.Log(other.name);
    }
}