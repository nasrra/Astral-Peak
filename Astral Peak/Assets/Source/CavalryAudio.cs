using UnityEngine;


public class CavalryAudio : MonoBehaviour
{
    AudioSource source;

    public void emit_arrow_knocked()      => AudioClipHandler.play(this, SoundLibrary.get_sound(SoundID.COIN_TOSS), out source);                                                    
    public void emit_bark()               => AudioClipHandler.play(this, choose_bark(), out source);                                                
    public void emit_bow_shot()           => AudioClipHandler.play(this, SoundLibrary.get_sound(SoundID.BOW_SHOT), out source);                                                        
    public void emit_front_strike_grab()  => AudioClipHandler.play(this, SoundLibrary.get_sound(SoundID.LEATHER_CONTORT_1), out source);                                                     
    public void emit_ground_slam_hop()    => AudioClipHandler.play(this, SoundLibrary.get_sound(SoundID.MAGIC_1), out source);                                                                      
    public void emit_ground_slam_impact() => AudioClipHandler.play(this, SoundLibrary.get_sound(SoundID.SNOW_IMPACT_HEAVY), out source);                                           
    public void emit_howl()               => AudioClipHandler.play(this, SoundLibrary.get_sound(SoundID.WOLF_HOWL), out source);   
    public void emit_sword_strike()       => AudioClipHandler.play(this, SoundLibrary.get_sound(SoundID.MELEE_SWING_2), out source);                                                                            
    public void emit_footsteps()          => AudioClipHandler.play(this, choose_footstep(), out source);                                        

    public Sound choose_bark()
    {
        int x = Random.Range(0, 3);
        switch (x)
        {
            case 0: return SoundLibrary.get_sound(SoundID.DOG_BARK_1);
            case 1: return SoundLibrary.get_sound(SoundID.DOG_BARK_2);
            case 2: return SoundLibrary.get_sound(SoundID.DOG_BARK_3);
        }
        throw new System.Exception("ERROR!");        
    }

    public Sound choose_footstep()
    {
        int x = Random.Range(0, 4);
        switch (x)
        {
            case 0: return SoundLibrary.get_sound(SoundID.SNOW_FOOTSTEP_1);
            case 1: return SoundLibrary.get_sound(SoundID.SNOW_FOOTSTEP_2);
            case 2: return SoundLibrary.get_sound(SoundID.SNOW_FOOTSTEP_3);
            case 3: return SoundLibrary.get_sound(SoundID.SNOW_FOOTSTEP_4);
        }
        throw new System.Exception("ERROR!");
    }
}
