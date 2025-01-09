using System;
using System.Collections;
using System.Collections.Generic;
using Deluz;
using UnityEngine;

public class MageRoomPlatforms : MonoBehaviour{
    [SerializeField] GameObject platform;
    [SerializeField] List<Animator> animators;
    [SerializeField] ObjectOrbiter orbiter;
    [SerializeField] List<ObjectScroller> scrollers = new List<ObjectScroller>();

    void Start(){
        enable_scroller();
        //StartCoroutine(loop());   
    }

    IEnumerator loop(){
        while(true){
            yield return new WaitForSeconds(1);
            enable_orbiter();
            yield return new WaitForSeconds(5);
            disable_orbiter();
            yield return new WaitForSeconds(4);
        }
    }

    public void enable_scroller(){
        foreach(ObjectScroller scroller in scrollers){
            scroller.instantiate_objects();
            List<GameObject> platforms = scroller.get_objects();
            foreach(GameObject o in platforms)
                o.GetComponent<Animator>().Play("turn_on");
            scroller.start_scrolling();
        }
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
        foreach(Animator animator in animators){
            animator.Play("turn_off");
            Destroy(animator.gameObject, animator.GetCurrentAnimatorClipInfo(0).Length);
        }
        animators.Clear();
    }
}
