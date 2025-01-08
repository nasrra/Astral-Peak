using System.Collections;
using DocumentFormat.OpenXml.Packaging;
using Unity.VisualScripting;
using UnityEngine;

namespace Cutscenes{
    public class MageOpening : Cutscene{
        public override IEnumerator get_coroutine(){
            MageBossRoom room = BossRoomHandler.instance as MageBossRoom;
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
            Mage mage = room.get_mage();
            mage.link_phase("phase_1");
            CameraController.instance.set_target(mage.transform);
            CameraController.instance.lerp_offset(x:null, y:-3.5f, time:.15f);
            yield return new WaitForSeconds(1f);
            mage.animator.Play("MageYell");
            yield return new WaitForSeconds(1f);
            CameraController.instance.reset_offset(2f);
            CameraController.instance.reset_zoom(2f);
            yield return new WaitForSeconds(3);
            CameraController.instance.set_target(Player.instance.transform);
            end();//
            yield break;
        }
    }
    public class MagePhaseTransition : Cutscene{            
        public override IEnumerator get_coroutine(){
            MageBossRoom room = BossRoomHandler.instance as MageBossRoom;
            Mage mage = (BossRoomHandler.instance as MageBossRoom).get_mage();
            CameraEffects.instance.fade_to_black(fade_transition_time);
            room.enable_mage(false);
            yield return new WaitForSeconds(fade_transition_time);
            prepare();
            CameraEffects.instance.fade_from_black(fade_transition_time);
            yield return new WaitForSeconds(fade_transition_time);
            mage.animator.Play("MageYell");
            CameraController.instance.set_target(mage.transform);
            yield return new WaitForSeconds(mage.animator.get_clip_length("MageYell")+1);
            room.set_room_state(1);
            mage.link_phase("phase_2");
            mage.animator.Play("MagePhaseTransition");
            mage.unlink_movement();
            mage.get_movement().mod_speed(3);
            mage.get_movement().mod_gravity(0);
            mage.get_movement().freeform_approach_to(room.get_boss_point(2));
            room.emit_attraction_particles();
            yield return new WaitForSeconds(6);
            mage.get_movement().reset_speed();
            mage.get_movement().clear_move_direction();
            mage.get_movement().zero_velocity();
            room.stop_attraction_particles();
            yield return new WaitForSeconds(3);
            CameraController.instance.set_target(Player.instance.transform);
            end();
            yield break;
            //set_fly_pattern();
        }

        void prepare(){
            MageBossRoom room = BossRoomHandler.instance as MageBossRoom;
            Mage mage = (BossRoomHandler.instance as MageBossRoom).get_mage();
            mage.transform.position = room.get_boss_point(1).position;
            room.set_respawn_point();
            Player.instance.set_enter_position();
            room.enable_mage(true);
            mage.unlink_phase("phase_1");
            mage.link_phase("cutscene_transition");
            mage.animator.Play("MageIdle",0,0);
        }
    }
}

