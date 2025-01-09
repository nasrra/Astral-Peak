using UnityEngine;

public class Platform : MonoBehaviour{
    public void remove_player(){
        // if we contain player, remove it from us.
        if(Player.instance.transform.parent == transform)
            Player.instance.transform.parent = null; 
    }
    public void disable(){
        remove_player();
        gameObject.SetActive(false);
    }

    public void enable(){
        gameObject.SetActive(true);
    }
}
