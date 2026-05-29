using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnPointManager{
    static Dictionary<string,SpawnPoint> points = new Dictionary<string,SpawnPoint>();
    public static void add_point(SpawnPoint point) => points.Add(point.name,point);
    public static void erase_point(SpawnPoint point) => points.Remove(point.name);
    public static SpawnPoint get_point(string point_name) => contains_point(point_name)? points[point_name] : null;  
    public static bool contains_point(string point) => points.ContainsKey(point);  
}
