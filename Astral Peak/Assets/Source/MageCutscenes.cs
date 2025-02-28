using System.Collections;
using UnityEngine;

namespace Cutscenes{
    public class MageOpening : Cutscene{
        public override IEnumerator get_coroutine(){
            MageBossRoom room = RoomHandler.instance as MageBossRoom;
            Mage mage = room.mage;
            BackgroundMage background_mage = room.background_mage;
            CameraController.instance.lerp_offset(x:null, y:0.9f, time:2f);
            CameraController.instance.lerp_zoom(size:8.65f, time:2f);
            yield return new WaitForSeconds(2.5f);
            background_mage.gameObject.SetActive(true);
            background_mage.teleport(0);
            yield return new WaitForSeconds(2+background_mage.teleport_time);
            background_mage.teleport(1);
            yield return new WaitForSeconds(background_mage.teleport_time/2);
            CameraController.instance.set_target(background_mage.transform);
            background_mage.flip();
            yield return new WaitForSeconds(1);
            background_mage.teleport(2);
            yield return new WaitForSeconds(background_mage.teleport_time/2);
            background_mage.flip();
            yield return new WaitForSeconds(1);
            background_mage.teleport(3);
            yield return new WaitForSeconds(background_mage.teleport_time/2);
            background_mage.enable_sprite(false);
            background_mage.destroy();
            mage.gameObject.SetActive(true);
            // mage.get_sprite().play_death_effect_reverse(1);
            mage.flip_to_target();
            mage.link_phase("phase_1");
            CameraController.instance.set_target(mage.gameObject.transform);
            CameraController.instance.lerp_offset(x:null, y:-3.5f, time:.15f);
            yield return new WaitForSeconds(1f);
            mage.animator.Play("MageYell");
            yield return new WaitForSeconds(1f);
            CameraController.instance.reset_offset(2f);
            CameraController.instance.reset_zoom(2f);
            yield return new WaitForSeconds(2);
            stop_skip();
            yield return new WaitForSeconds(1);
            CameraController.instance.set_target(Player.instance.transform);
            AudioManager.play_music("music_the_mage_1");
            end();//
            yield break;
        }
    }
    public class MagePhaseTransition : Cutscene{            
        public override IEnumerator get_coroutine(){
            MageBossRoom room = RoomHandler.instance as MageBossRoom;
            Mage mage = room.mage;
            CameraEffects.instance.fade_to_black(fade_transition_time);
            mage.gameObject.SetActive(false);
            yield return new WaitForSeconds(fade_transition_time);
            prepare();
            CameraEffects.instance.fade_from_black(fade_transition_time);
            yield return new WaitForSeconds(fade_transition_time);
            mage.animator.Play("MageYell");
            CameraController.instance.set_target(mage.transform);
            yield return new WaitForSeconds(mage.animator.get_clip_length("MageYell")+1);
            mage.animator.Play("MagePhaseTransition1",0,0);
            yield return new WaitForSeconds(mage.animator.get_clip_length("MagePhaseTransition1")/2);
            room.emit_attraction_particles();
            yield return new WaitForSeconds(mage.animator.get_clip_length("MagePhaseTransition1")/2);
            room.set_room_state(1);
            mage.link_phase("phase_2");
            mage.animator.Play("MagePhaseTransition2",0,0);
            mage.phase_2_body_charged();
            mage.get_movement().mod_speed(2);
            mage.get_movement().freeform_approach_to(room.get_boss_point(2));
            CameraController.instance.lerp_offset(null, 6,8);
            yield return new WaitForSeconds(8);
            room.stop_attraction_particles();
            yield return new WaitForSeconds(4);
            CameraController.instance.reset_offset(2);
            mage.get_movement().reset_speed();
            mage.get_movement().clear_move_direction();
            mage.get_movement().zero_velocity();
            yield return new WaitForSeconds(2);
            stop_skip();
            yield return new WaitForSeconds(1);
            CameraController.instance.set_target(Player.instance.transform);
            room.get_platforms().start_loop();
            AudioManager.play_music("music_the_mage_2");
            room.start_randomised_stone_lightning();
            room.enable_button_prompt();
            end();
            yield break;
        }

        void prepare(){
            MageBossRoom room = RoomHandler.instance as MageBossRoom;
            Mage mage = room.mage;
            mage.transform.position = room.get_boss_point(1).position;
            Player.instance.set_enter_position();
            mage.flip_to_target();
            room.set_respawn_point(1);
            room.mage.gameObject.SetActive(true);
            mage.unlink_phase("phase_1");
            mage.link_phase("cutscene_transition");
            mage.animator.Play("MageIdle",0,0);
        }
    }
}

