using DocumentFormat.OpenXml.Packaging;
using UnityEngine;

public class ShrineCutsceneConstellations : MonoBehaviour{
    [SerializeField] ConstellationController
        world, gateway, aether, soul;
    
    void Start(){
        ShrineCutscene.world_constellation_on       += world.fade_in;
        ShrineCutscene.world_constellation_off      += world.fade_out;
        ShrineCutscene.gateway_constellation_on     += gateway.fade_in;
        ShrineCutscene.gateway_constellation_off    += gateway.fade_out;
        ShrineCutscene.aether_constellation_on      += aether.fade_in;
        ShrineCutscene.aether_constellation_off     += aether.fade_out;
        ShrineCutscene.soul_constellation_on        += soul.fade_in;
        ShrineCutscene.soul_constellation_off       += soul.fade_out;
    }

    void OnDestroy(){
        ShrineCutscene.world_constellation_on       -= world.fade_in;
        ShrineCutscene.world_constellation_off      -= world.fade_out;
        ShrineCutscene.gateway_constellation_on     -= gateway.fade_in;
        ShrineCutscene.gateway_constellation_off    -= gateway.fade_out;
        ShrineCutscene.aether_constellation_on      -= aether.fade_in;
        ShrineCutscene.aether_constellation_off     -= aether.fade_out;
        ShrineCutscene.soul_constellation_on        -= soul.fade_in;
        ShrineCutscene.soul_constellation_off       -= soul.fade_out;     
    }

}
