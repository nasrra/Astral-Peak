using System;
using AYellowpaper.SerializedCollections;
using Entropek;
using UnityEngine;

public class MeleeHolsterHandler : MonoBehaviour{
    public event Action hit;
    public event Action<KnockbackData> self_knockback;
    [SerializedDictionary("id","melee holster")]
    [SerializeField] SerializedDictionary<string, MeleeHolster> holsters = new SerializedDictionary<string, MeleeHolster>();
    public void enable_melee_hurtbox(string holster_id){
        holsters[holster_id].enable_hurt_box(true);
    }
    public void disable_melee_hurtbox(string holster_id){
        holsters[holster_id].enable_hurt_box(false);
    } 


    void Awake(){
        foreach(MeleeHolster h in holsters.Values){
            h.hit_creature      += hit_creature;
            h.self_knockback    += invoke_self_knockback;
        }
    }
    void OnDestroy(){
        foreach(MeleeHolster h in holsters.Values){
            h.hit_creature      -= hit_creature;
            h.self_knockback    -= invoke_self_knockback;
        }
    }
    void hit_creature() => hit?.Invoke();
    void invoke_self_knockback(KnockbackData data) => self_knockback?.Invoke(data);
}
