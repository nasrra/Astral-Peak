using UnityEngine;

public class Movement : MonoBehaviour{
    [SerializeField] private Vector3 move_direction = new Vector3();

    public void move_left(bool x){
        move_direction += (x == true)? new Vector3(-1,0,0) : new Vector3(1,0,0);
    }

    public void move_right(bool x){
        move_direction += (x == true)? new Vector3(1,0,0) : new Vector3(-1,0,0);
    }

    public void move_up(bool x){
        move_direction += (x == true)? new Vector3(0,1,0) : new Vector3(0,-1,0);
    }
}
