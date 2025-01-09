using Deluz;
using Deluz.Collections;
using UnityEngine;

public class ProjectileManager : MonoBehaviour{
    public static ProjectileManager instance;
    [SerializeField] SwapbackArray<Projectile> projectiles = new SwapbackArray<Projectile>();
    //[SerializeField] List<Projectile> p = new List<Projectile>();
    void Awake(){
        instance = this;
        link_events();   
    }
    void OnDestroy(){
        instance = null;
        unlink_events();
    }
    public void add_projectile(Projectile projectile){
        projectiles.Add(projectile);
        //p = projectiles;
    }
    public void remove_projectile(Projectile projectile){
        projectiles.Remove(projectile);
        //p = projectiles;
    } 
    public void destroy_all_projectiles(){
        Log.MethodCall(this);
        for(int i = 0; i < projectiles.Count; ++i)
            Destroy(projectiles[i].gameObject);
        projectiles.Clear();
        //p=projectiles;
    }
    void entered_game_state(GameState state){
        if(state==GameState.CUTSCENE)
            destroy_all_projectiles();
    }
    void link_events() => GameManager.entered_game_state += entered_game_state;
    void unlink_events() => GameManager.entered_game_state -= entered_game_state;
}
