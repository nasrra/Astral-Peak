using Deluz.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour{
    public static EnemyManager instance;
    SwapbackArray<Enemy> enemies = new SwapbackArray<Enemy>();
    void Awake(){
        instance = this;
        link_events();   
    }
    void OnDestroy(){
        instance = null;
        unlink_events();
    }
    public void add(Enemy enemy){
        enemies.Add(enemy);
    }
    public void remove(Enemy enemy){
        enemies.Remove(enemy);
    }
    public void destroy_all(){
        for(int i = 0; i < enemies.Count; ++i)
            Destroy(enemies[i].gameObject);
        enemies.Clear();
    }
    void entered_game_state(GameState state){
        if(state==GameState.CUTSCENE)
            destroy_all();
    }
    void link_events() => GameManager.entered_game_state += entered_game_state;
    void unlink_events() => GameManager.entered_game_state -= entered_game_state;
}
