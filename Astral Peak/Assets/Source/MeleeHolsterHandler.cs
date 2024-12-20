using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class MeleeHolsterHandler : MonoBehaviour{
    public event Action hit;
    [SerializedDictionary("id","melee holster")]
    [SerializeField] SerializedDictionary<string, MeleeHolster> holsters = new SerializedDictionary<string, MeleeHolster>();
    public void enable_melee_hurtbox(string holster_id) => holsters[holster_id].enable_hurt_box(true);
    public void disable_melee_hurtbox(string holster_id) => holsters[holster_id].enable_hurt_box(false);

    void hit_creature() => hit?.Invoke();

    void Awake(){
        foreach(MeleeHolster h in holsters.Values)
            h.hit_creature += hit_creature;
    }
    void OnDestroy(){
        foreach(MeleeHolster h in holsters.Values)
            h.hit_creature -= hit_creature;
    }
}
