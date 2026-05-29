using System;

[Serializable]
public class BossAttack{
    public BossAttack(string animation_id, float max_player_distance, float min_player_distance, float arena_bound_distance, float attack_cooldown, float combat_cooldown, float idle_cooldown = 0, bool enabled = true){
        this.animation_id = animation_id;
        this.max_player_distance = max_player_distance;
        this.min_player_distance = min_player_distance;
        this.arena_bound_distance = arena_bound_distance;
        this.attack_cooldown = attack_cooldown;
        this.combat_cooldown = combat_cooldown;
        this.idle_cooldown = idle_cooldown;
        this.enabled = enabled;
    } 

    public string animation_id;
    public float 
        max_player_distance,
        min_player_distance,
        arena_bound_distance,
        attack_cooldown,
        combat_cooldown,
        idle_cooldown;
    public bool enabled = true;
}
