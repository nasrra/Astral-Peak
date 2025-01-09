using System.Collections.Generic;
using UnityEngine;

// Remember to uncheck "Write Defaults" in the animator controller to avoid any funky business.
// may also need to start adding states that go for one frame that "Write's Defaults" for a animator then goes to the idle state.

public class AnimatorOverride : MonoBehaviour{
    [SerializeField] protected Animator animator;
    Dictionary<string, AnimationClip> clips = new Dictionary<string, AnimationClip>();
    void Awake() => create_clip_dictionary();
    public void create_clip_dictionary(){
        foreach(AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            clips.Add(clip.name, clip);
    }
    public float get_clip_length(string id) => clips[id].length;
    public void StopPlayback() => animator.StopPlayback();
    public void Play(int id) => animator.Play(id);
    public void Play(int id, int layer) => animator.Play(id,layer);
    public void Play(int id, int layer, int normalized_time) => animator.Play(id,layer,normalized_time);
    public void Play(string id) => animator.Play(id);
    public void Play(string id, int layer) => animator.Play(id,layer);
    public void Play(string id, int layer, int normalized_time) => animator.Play(id,layer,normalized_time);
    public void Rebind() => animator.Rebind();
    public void UpdateLayer(int layer) => animator.Update(layer);
}