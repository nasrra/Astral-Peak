using Entropek.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour{
    public static EnemyManager instance;
    [SerializeField] private bool start_all_inactive = false;
    SwapbackArray<Enemy> enemies = new SwapbackArray<Enemy>();
    void Awake(){
        instance = this;
        link_events();   
    }
    void Start(){
        if(start_all_inactive == true)
            set_active_all(false);
    }
    void OnDestroy(){
        instance = null;
        unlink_events();
        AudioManager.unload_bank("entity_hollow");
    }
    public void add(Enemy enemy){
        check_load_bank();
        enemies.Add(enemy);
    }
    public void remove(Enemy enemy){
        enemies.Remove(enemy);
        check_unload_bank();
    }
    public void destroy_all(){
        for(int i = 0; i < enemies.Count; ++i)
            Destroy(enemies[i].gameObject);
        enemies.Clear();
        check_unload_bank();
    }
    public void set_active_all(bool _active){
        for(int i = 0; i < enemies.Count; ++i)
            enemies[i].gameObject.SetActive(_active);
    }
    void entered_game_state(GameState state){
        if(state==GameState.CUTSCENE)
            destroy_all();
    }
    public void set_start_all_inactive(bool _active) => start_all_inactive = _active;
    void link_events() => GameManager.entered_game_state += entered_game_state;
    void unlink_events() => GameManager.entered_game_state -= entered_game_state;

    void check_load_bank(){
        if(enemies.Count == 0)
            AudioManager.load_bank("entity_hollow");
    }

    void check_unload_bank(){
        if(enemies.Count == 0)
            AudioManager.unload_bank("entity_hollow");
    }
}
