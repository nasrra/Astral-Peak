using UnityEngine;
using AYellowpaper.SerializedCollections;
using System;

public class MeleeHolsterHandler : MonoBehaviour{
    [SerializedDictionary("attack", "collider")]
    [SerializeField] SerializedDictionary<string, MeleeHolster> hurtboxes = new SerializedDictionary<string, MeleeHolster>();
    public void toggle_hurt_box(string melee_holster, bool enabled) => hurtboxes[melee_holster].enable_hurt_box(enabled);
}
