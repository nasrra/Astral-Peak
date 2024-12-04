using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderMovement : Movement{
    // animator functions
    public void forward_strike_lunge()      => dash(transform.rotation.y == 0? Vector2.right : Vector2.left, 20, 0.30f);
    public void signature_strike_lunge()    => dash(transform.rotation.y == 0? Vector2.right : Vector2.left, 20, 0.30f);
    public void jump_n_dash_jump_back()     => dash(transform.rotation.y == 0? Vector2.left : Vector2.right, 20, 0.40f);
    public void jump_n_dash_front_leap()    => dash(transform.rotation.y == 0? Vector2.right : Vector2.left, 40, 0.30f);
}
