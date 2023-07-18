using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelSettings", menuName = "ScriptableObjects/GameLevel", order = 1)]
public class GameLevel : ScriptableObject
{
    //this holds level specific conditions

    public bool istimed;
    public float timelimit;
    public bool requirepickup;
    public pickuptype type;
    public float pickupcount;

}