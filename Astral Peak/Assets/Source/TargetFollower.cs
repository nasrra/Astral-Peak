using UnityEngine;

public class TargetFollower : MonoBehaviour{
    [SerializeField] Transform target;
    [SerializeField] Vector2 x_limits;
    [SerializeField] Vector2 y_limits;
    [SerializeField] Vector3 offset;

    // Update is called once per frame
    void FixedUpdate(){
        Vector3 desired_pos = target.position + offset;
        if(desired_pos.x <= x_limits.x)   
            desired_pos.x = x_limits.x;
        else if(desired_pos.x >= x_limits.y)
            desired_pos.x = x_limits.y;
        if(desired_pos.y <= y_limits.x)   
            desired_pos.y = y_limits.x;
        else if(desired_pos.y >= y_limits.y)
            desired_pos.y = y_limits.y;
        transform.position = desired_pos;
    }
}
