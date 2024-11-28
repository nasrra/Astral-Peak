using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DoorManager{
    static private Dictionary<string, Door> doors = new Dictionary<string, Door>();

    static public void add_door(Door door) => doors.Add(door.gameObject.name, door);
    static public void erase_door(Door door) => doors.Remove(door.gameObject.name);

    static public Vector3 get_position(string exit_point){
        Door door = doors[exit_point];
        return door != null? door.get_enter_point().position : throw new NullReferenceException(SceneManager.GetActiveScene().name + " does not contain exit point: " + exit_point);
    }

    static public Door get_door(string exit_point){
        Door door = doors[exit_point];
        return door != null? door : throw new NullReferenceException(SceneManager.GetActiveScene().name + " does not contain exit point: " + exit_point);
    }
}
