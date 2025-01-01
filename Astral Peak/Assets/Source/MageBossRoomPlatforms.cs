using System;
using System.Collections.Generic;
using UnityEngine;

public class MageBossRoomPlatforms : MonoBehaviour{
    MovingObjects circular_movement;
    Dictionary<int, Action> platform_behaviours;
    Dictionary<int, Action> create_behaviours() =>
        new Dictionary<int, Action>(){
            {0,()=>{
                circular_movement.set_behaviour(MovingObjectsBehavior.CLOCKWISE_CIRCLE);
                circular_movement.gameObject.SetActive(true);
            }}
        };
}
