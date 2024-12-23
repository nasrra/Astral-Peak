using UnityEngine;

public struct AudioSourceSettings{
    public readonly bool randomise_pitch, spatial_blend, loop;
    AudioSourceSettings(bool _randomise_pitch, bool _spatial_blend, bool _loop){
        randomise_pitch    = _randomise_pitch;
        spatial_blend      = _spatial_blend;
        loop               = _loop;
    }


    // diegetic sounds that occur in the game world.
    public static readonly AudioSourceSettings DIEGETIC = new AudioSourceSettings(
        _spatial_blend: true,
        _randomise_pitch: false,
        _loop: false
    );
    public static readonly AudioSourceSettings DIEGETIC_RANDOMISED = new AudioSourceSettings(
        _spatial_blend: true,
        _randomise_pitch: true,
        _loop: false        
    );
    public static readonly AudioSourceSettings DIEGETIC_LOOP = new AudioSourceSettings(
        _spatial_blend: true,
        _randomise_pitch: false,
        _loop: true        
    );
    public static readonly AudioSourceSettings DIEGETIC_RANDOMISED_LOOP = new AudioSourceSettings(
        _randomise_pitch: true,
        _spatial_blend: true,
        _loop: true        
    );

    // non diegetic sound for things like ui and music.
    public static readonly AudioSourceSettings NON_DIEGETIC = new AudioSourceSettings(
        _spatial_blend: false,
        _randomise_pitch: false,
        _loop: false
    );
    public static readonly AudioSourceSettings NON_DIEGETIC_RANDOMISED = new AudioSourceSettings(
        _spatial_blend: false,
        _randomise_pitch: true,
        _loop: false        
    );
    public static readonly AudioSourceSettings NON_DIEGETIC_LOOP = new AudioSourceSettings(
        _spatial_blend: false,
        _randomise_pitch: false,
        _loop: true        
    );
    public static readonly AudioSourceSettings NON_DIEGETIC_RANDOMISED_LOOP = new AudioSourceSettings(
        _spatial_blend: false,
        _randomise_pitch: true,
        _loop: true        
    );
}