using System.Collections.Generic;
using UnityEngine;

public class EnemyDoorLock : MonoBehaviour{
    [SerializeField] List<Enemy> enemies = new List<Enemy>();
    [SerializeField] int enemy_kill_requirement;
    [SerializeField] Door door;
    private int count;
    void Start() => link_events();
    void OnDestroy() => unlink_events();
    
    void handle_enemy_death(Enemy enemy){
        ++count;
        if(count >= enemy_kill_requirement){
            door.open();
            unlink_events();
        }
    }

    void link_enemy(Enemy e) => e.enemy_death += handle_enemy_death;
    void unlink_enemy(Enemy e) =>  e.enemy_death -= handle_enemy_death;

    void link_events(){
        foreach(Enemy e in enemies)
            link_enemy(e);
    }
    void unlink_events(){
        foreach(Enemy e in enemies)
            unlink_enemy(e);
        enemies.Clear();
    }
}
