public static class LayersManager{
    // used for checking layers in colliders
    public static readonly int
        HURTBOX                         = 3,
        GROUND                          = 6,
        PLAYER                          = 7,
        ENEMY                           = 8,
        PROJECTILE                      = 9,
        PLATFORM                        = 10,
        PARTICLE_ATTRACTOR              = 11,
        KILLZONE                        = 12,
        BOSS                            = 13,
        ROPE                            = 14,
        PROJECTILE_DESTROYER            = 15,
        LEVEL_BOUND                     = 16,
        BITWISE_HURTBOX                 = 1 << HURTBOX,        
        BITWISE_GROUND                  = 1 << GROUND,
        BITWISE_PLAYER                  = 1 << PLAYER,
        BITWISE_ENEMY                   = 1 << ENEMY,
        BITWISE_PROJECTILE              = 1 << PROJECTILE,
        BITWISE_PLATFORM                = 1 << PLATFORM,
        BITWISE_PARTICLE_ATTRACTOR      = 1 << PARTICLE_ATTRACTOR,
        BITWISE_KILLZONE                = 1 << KILLZONE,
        BITWISE_BOSS                    = 1 << BOSS,
        BITWISE_ROPE                    = 1 << ROPE,
        BITWISE_PROJECTILE_DESTROYER    = 1 << PROJECTILE_DESTROYER,
        BITWISE_LEVEL_BOUND             = 1 << LEVEL_BOUND
        ;
}
