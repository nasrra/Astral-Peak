using System.Collections;
using UnityEngine;

namespace Cutscenes{
    public class MageOpening : Cutscene{
        public override IEnumerator get_coroutine(){
            MageBossRoom room = BossRoomHandler.instance as MageBossRoom;
            Mage mage = room.get_mage(); 
            MageBackground background_mage = room.get_background_mage();
            CameraController.instance.lerp_offset(x:null, y:0.9f, time:2f);
            CameraController.instance.lerp_zoom(size:8.65f, time:2f);
            yield return new WaitForSeconds(2.5f);
            room.enable_background_mage(true);
            background_mage.teleport(0);
            yield return new WaitForSeconds(2+background_mage.teleport_time);
            background_mage.teleport(1);
            yield return new WaitForSeconds(background_mage.teleport_time/2);
            CameraController.instance.set_target(background_mage.transform);
            yield return new WaitForSeconds(1);
            background_mage.teleport(2);
            yield return new WaitForSeconds(1+background_mage.teleport_time/2);
            background_mage.teleport(3);
            yield return new WaitForSeconds(background_mage.teleport_time/2);
            background_mage.enable_sprite(false);
            background_mage.destroy();
            room.enable_mage(true);
            CameraController.instance.set_target(mage.transform);
            CameraController.instance.lerp_offset(x:null, y:-3.5f, time:.15f);
            yield return new WaitForSeconds(1f);
            mage.animator.Play("MageYell");
            yield return new WaitForSeconds(1f);
            CameraController.instance.reset_offset(2f);
            CameraController.instance.reset_zoom(2f);
            yield return new WaitForSeconds(3);
            CameraController.instance.set_target(Player.instance.transform);
            end();
            yield break;
        }
    }
    public class MagePhaseTransition : Cutscene{
        public override IEnumerator get_coroutine(){
            MageBossRoom room = BossRoomHandler.instance as MageBossRoom;
            room.emit_attraction_particles();
            yield return new WaitForSeconds(1);
            room.stop_attraction_particles();
            room.enable_mage(true);
            yield return new WaitForSeconds(1);
            end();
            yield break;
        }
    }
}

