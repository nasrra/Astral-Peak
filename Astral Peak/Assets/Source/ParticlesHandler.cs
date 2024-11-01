using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticlesHandler : MonoBehaviour{
    protected Vector3 left = new Vector3(1,0,0);
    protected Vector3 right = new Vector3(0,0,0);
    public abstract void flip_left();
    public abstract void flip_right();
}


