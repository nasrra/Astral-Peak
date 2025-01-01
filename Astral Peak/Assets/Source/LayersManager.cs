using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayersManager{
    // used for checking layers in colliders
    public static readonly int
        HURTBOX             = 3,
        GROUND              = 6,
        PLAYER              = 7,
        ENEMY               = 8,
        PROJECTILE          = 9,
        PLATFORM            = 10,
        BITWISE_HURTBOX     = 1 << HURTBOX,        
        BITWISE_GROUND      = 1 << GROUND,
        BITWISE_PLAYER      = 1 << PLAYER,
        BITWISE_ENEMY       = 1 << ENEMY,
        BITWISE_PROJECTILE  = 1 << PROJECTILE,
        BITWISE_PLATFORM    = 1 << PLATFORM;

}
