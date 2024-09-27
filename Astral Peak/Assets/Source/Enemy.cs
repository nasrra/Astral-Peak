using UnityEngine;

public class Enemy : Creature{
    [Header("Enemy")]
    [SerializeField] protected AiPathFollow path_follow;
    public void follow_path(bool x) => path_follow.enabled = x;
}