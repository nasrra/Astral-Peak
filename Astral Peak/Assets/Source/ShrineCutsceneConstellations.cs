using DocumentFormat.OpenXml.Packaging;
using UnityEngine;

public class ShrineCutsceneConstellations : MonoBehaviour{
    [SerializeField] ConstellationController
        world, gateway, aether, soul;
    
    void Start() => link(); 
    void OnDestroy() => unlink();

    void handle_cutscene(Cutscene cutscene){
        switch(cutscene){
            case ShrineOpeningCutscene c:
                c.world_constellation_on       += world.fade_in;
                c.world_constellation_off      += world.fade_out;
                c.gateway_constellation_on     += gateway.fade_in;
                c.gateway_constellation_off    += gateway.fade_out;
                c.aether_constellation_on      += aether.fade_in;
                c.aether_constellation_off     += aether.fade_out;
                c.soul_constellation_on        += soul.fade_in;
                c.soul_constellation_off       += soul.fade_out;    
                break;
        }
    }

    void link() => CutsceneManager.started_cutscene += handle_cutscene;
    void unlink() => CutsceneManager.started_cutscene -= handle_cutscene;

}
