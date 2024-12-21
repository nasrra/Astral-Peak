using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class RangedHolsterHandler : MonoBehaviour{
    public event Action<string> fired;
    [SerializedDictionary("id","ranged holster")]
    [SerializeField] SerializedDictionary<string, RangedHolster> holsters = new SerializedDictionary<string, RangedHolster>();
    public void fire_projectile(string holster_id){
        holsters[holster_id].fire_once();
        fired?.Invoke(holster_id);
    }
    public RangedHolster get_holster(string id) => holsters[id];
}
