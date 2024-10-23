using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour{
    public static EnemyManager instance;
    public UnityEvent no_enemies;
    [SerializeField] int alive_enemies;

    void Awake() => instance = this;

    public void add_enemy() => alive_enemies += 1;
    public void remove_enemy(){
        alive_enemies -= 1;
        if(alive_enemies <= 0)
            no_enemies?.Invoke();
    }
}
