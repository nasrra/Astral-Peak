using System.Collections;
using TreeEditor;
using UnityEngine;

public class SludgeBeam : SludgeGeyser{
    public override void turn_on(){
        play_bust_sound();
        StartCoroutine("calcuate_length");
        base.turn_on();
    }

    public override void turn_off(){
        StopCoroutine("calcuate_length");
        base.turn_off();
    }

    public override void update_length(float _length){
        line.SetPosition(1, new Vector3(0,0,_length*2.5f));
        hurtbox.size    = new Vector2(hurtbox.size.x,_length*2.5f);
        hurtbox.offset  = new Vector2(0,_length*2.5f/2);
        end_particle.transform.position = transform.position + transform.up * _length;
    }

    IEnumerator calcuate_length(){
        while(true){
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 100, LayersManager.BITWISE_GROUND);
            if(hit == true){
                update_length((hit.point - new Vector2(transform.position.x,transform.position.y)).magnitude);
            }
            else
                update_length(2);
            yield return new WaitForFixedUpdate();
        }
    }
}
