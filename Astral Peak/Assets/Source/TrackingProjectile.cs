using System;
using System.Collections;
using Entropek;
using UnityEngine;

//public class TrackingProjectile{
//    public event Action buffer_started, buffer_stopped;
//    [SerializeField] protected float
//        buffer_time,
//        snap_rotate_speed,
//        lerp_rotate_speed;
//    [SerializeField] bool enable_col_before_buffer = true;
//    Coroutine buffer_state;
//
//    void Awake(){
//        state_switch(ref rotate_state, rotate_to_target(Player.instance.transform, snap_rotate_speed));
//        buffer();
//    }
//
//    public void buffer() =>
//        state_switch(ref buffer_state, Util.timer(buffer_time,
//            start_action: () => {
//                enable_colliders(enable_col_before_buffer);   
//                buffer_started?.Invoke();
//            },
//            time_out: () => {
//                state_switch(ref move_state, move(move_speed));
//                state_switch(ref rotate_state, rotate_to_target(Player.instance.transform, lerp_rotate_speed));
//                enable_trail(true);
//                enable_colliders(true);
//                buffer_stopped?.Invoke();
//                transform.parent = null;
//        }));
//}
