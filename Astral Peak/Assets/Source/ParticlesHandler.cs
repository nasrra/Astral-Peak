using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticlesHandler : MonoBehaviour{
    public abstract void flip_left();
    public void flip_emitter_left(ParticleSystemRenderer emitter) => emitter.flip = new Vector3(1, emitter.flip.y, 0);
    public void flip_emitter_right(ParticleSystemRenderer emitter) => emitter.flip = new Vector3(0, emitter.flip.y, 0);
    public abstract void flip_right();
}


