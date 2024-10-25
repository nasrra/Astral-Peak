using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour{
    public static DoorManager instance;
    [SerializeField] private List<Door> doors;
    void Awake(){
        instance = this;
    }

    public Vector3 get_position(string exit_point){
        foreach(Door d in doors)
            if(d.get_enter_point() == exit_point)
                return d.transform.position;
        throw new NullReferenceException(SceneManager.GetActiveScene().name + " does not contain exit point: " + exit_point);
    }

    public Door get_door(string door_name){
        foreach (Door d in doors)
            if(d.get_enter_point() == door_name)
                return d;
        throw new NullReferenceException(SceneManager.GetActiveScene().name + " does not contain door: " + door_name);
    }
}
