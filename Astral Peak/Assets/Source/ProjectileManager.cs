using Entropek.Collections;
using UnityEngine;

public class ProjectileManager : MonoBehaviour{
    public static ProjectileManager instance;
    [SerializeField] SwapbackArray<Projectile> projectiles = new SwapbackArray<Projectile>();
    void Awake(){
        instance = this;
        link_events();   
    }
    void OnDestroy(){
        instance = null;
        unlink_events();
    }
    public void add(Projectile projectile) => projectiles.Add(projectile);
    public void remove(Projectile projectile) => projectiles.Remove(projectile);
    public void destroy_all(){
        for(int i = 0; i < projectiles.Count; ++i)
            Destroy(projectiles[i].gameObject);
        projectiles.Clear();
    }
    void entered_game_state(GameState state){
        if(state==GameState.CUTSCENE)
            destroy_all();
    }
    void link_events(){
        GameManager.entered_game_state += entered_game_state;
        CustomSceneManager.preparing_scene_load += destroy_all;
    }
    void unlink_events(){
        GameManager.entered_game_state -= entered_game_state;
        CustomSceneManager.preparing_scene_load -= destroy_all; 
    }
}
