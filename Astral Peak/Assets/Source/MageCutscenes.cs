using System.Collections;
using UnityEngine;

namespace Cutscenes{
    public class MageOpening : Cutscene{
        public override IEnumerator get_coroutine(){
            MageBossRoom room = BossRoomHandler.instance as MageBossRoom;
            TheMage mage = room.get_mage(); 
            MageBackground background_mage = room.get_background_mage();
            CameraController.instance.lerp_offset(x:null, y:0.9f, time:2f);
            CameraController.instance.lerp_zoom(size:8.65f, time:2f);
            yield return new WaitForSeconds(2.5f);
            play_teleport_sound();
            yield return new WaitForSeconds(.5f);
            room.enable_background_mage(true);
            background_mage.teleport_exit();
            yield return new WaitForSeconds(2);
            background_mage.teleport_enter();
            play_teleport_sound();
            yield return new WaitForSeconds(.5f);
            CameraController.instance.set_target(background_mage.transform);
            background_mage.teleport_to_point(0);
            background_mage.teleport_exit();
            yield return new WaitForSeconds(.5f);
            background_mage.teleport_enter();
            play_teleport_sound();
            yield return new WaitForSeconds(.5f);
            background_mage.teleport_to_point(1);
            background_mage.teleport_exit();
            yield return new WaitForSeconds(.5f);
            play_teleport_sound();
            background_mage.teleport_enter();
            yield return new WaitForSeconds(.5f);
            background_mage.teleport_to_point(2);
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
            mage.idle(2);
            end();
            yield break;
        }
        void play_teleport_sound() =>
            AudioClipHandler.play(
            sound_id: Sounds.SoundID.ELECTRIC_BURST,
            audio_player: UnityHook.instance,
            AudioSourceSettings.NON_DIEGETIC_RANDOMISED);
    }
}

