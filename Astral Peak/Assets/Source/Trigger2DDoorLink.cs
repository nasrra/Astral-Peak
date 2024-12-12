using System.Collections.Generic;
using UnityEngine;

public class Trigger2DDoorLink : MonoBehaviour{
    [SerializeField] List<Door> doors = new List<Door>();
    void OnTriggerEnter2D(){
        foreach(Door d in doors)
            d.opened();
    }
}
