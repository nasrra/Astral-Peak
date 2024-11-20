using UnityEngine;


public class CavalryAudio : MonoBehaviour
{
    AudioSource source;

    public void emit_arrow_knocked()      => AudioClipHandler.play(this, SoundID.COIN_TOSS, out source);                                                    
    public void emit_bark()               => AudioClipHandler.play(this, SoundID.DOG_BARK_1, out source, randomise_pitch: true);                                                
    public void emit_bow_shot()           => AudioClipHandler.play(this, SoundID.BOW_SHOT, out source, randomise_pitch: true);                                                        
    public void emit_front_strike_grab()  => AudioClipHandler.play(this, SoundID.LEATHER_CONTORT_1, out source);                                                     
    public void emit_ground_slam_hop()    => AudioClipHandler.play(this, SoundID.MAGIC_1, out source);                                                                      
    public void emit_ground_slam_impact() => AudioClipHandler.play(this, SoundID.SNOW_IMPACT_HEAVY, out source);                                           
    public void emit_howl()               => AudioClipHandler.play(this, SoundID.WOLF_HOWL, out source);   
    public void emit_sword_strike()       => AudioClipHandler.play(this, SoundID.MELEE_SWING_2, out source);                                                                            
    public void emit_footsteps()          => AudioClipHandler.play(this, choose_footstep(), out source, randomise_pitch: true);  
    public void emit_magic_explosion()    => AudioClipHandler.play(this, SoundID.MAGIC_EXPLOSION, out source);                                      

    public SoundID choose_footstep()
    {
        int x = Random.Range(0, 4);
        switch (x)
        {
            case 0: return SoundID.SNOW_FOOTSTEP_1;
            case 1: return SoundID.SNOW_FOOTSTEP_2;
            case 2: return SoundID.SNOW_FOOTSTEP_3;
            case 3: return SoundID.SNOW_FOOTSTEP_4;
        }
        throw new System.Exception("ERROR!");
    }
}
