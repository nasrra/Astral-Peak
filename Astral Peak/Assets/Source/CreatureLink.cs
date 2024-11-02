using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreatureLink : MonoBehaviour{
    [SerializeField] Creature main;

    public Creature get_creature() => main;
}
