using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPlatformsController : MonoBehaviour{
    [SerializeField] GameObject platform;
    [SerializeField] List<Animator> animators;
    [SerializeField] ObjectOrbiter orbiter;
    [SerializeField] List<ObjectScroller> scrollers = new List<ObjectScroller>();


    public void start_loop() => StartCoroutine(loop());
    public void stop_loop(){
        StopAllCoroutines();
        foreach(ObjectScroller scroller in scrollers)
            scroller.StopAllCoroutines();
    }
    IEnumerator loop(){
        while(true){
            enable_scroller();
            yield return new WaitForSeconds(50);
            disable_scroller();
            reverse_scroller();
            yield return new WaitForSeconds(10);
        }
    }

    public void enable_scroller(){
        foreach(ObjectScroller scroller in scrollers){
            scroller.instantiate_objects();
            List<GameObject> platforms = scroller.get_objects();
            foreach(GameObject o in platforms){
                Animator a = o.GetComponent<Animator>();
                a.Play("turn_on");//
                animators.Add(a);
            }
            scroller.start_scrolling();
        }
    }//..//
    public void disable_scroller(){
        foreach(ObjectScroller scroller in scrollers)
            scroller.stop_scrolling();
        destroy_platforms();
    }
    public void reverse_scroller(){
        foreach(ObjectScroller scroller in scrollers)
            scroller.reverse_scroll();        
    }

    public void enable_orbiter(){
        if(orbiter.enabled == true)
            throw new Exception("orbiter already enabled!");
        orbiter.enabled = true;
        List<GameObject> objects = new List<GameObject>();
        for(int i = 0; i < 5; i++){
            GameObject p = Instantiate(platform,transform.position, Quaternion.identity);
            Animator animator = p.GetComponent<Animator>();
            animator.Play("turn_on");
            animators.Add(animator);
            objects.Add(p);
        }
        orbiter.set_objects(objects);
        orbiter.randomise_behaviour();
        orbiter.start_behaviour();
    }
    public void disable_orbiter(){
        orbiter.enabled = false;
        destroy_platforms();
    }
    public void destroy_platforms(){
        foreach(Animator animator in animators){
            animator.Play("turn_off");
            Destroy(animator.gameObject, animator.GetCurrentAnimatorClipInfo(0).Length);
        }
        animators.Clear();
    }
}
