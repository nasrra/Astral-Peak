using Unity.VisualScripting;
using UnityEngine;

public class Creature : MonoBehaviour{
    [Header("Creature")]
    [SerializeField] protected Health health;
    public Health get_health(){
        return health;
    }
}
