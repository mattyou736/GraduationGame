using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBattleAction : ScriptableObject {

    public new string name;
    public Vector2 baseDamage;
    public int apCost;

    [Header("Element")]
    public bool fire;
    public bool lighting;
    public bool water;
    public bool neutral;

    [Header("Property")]
    public bool physical;
    public bool ranged;
    public bool slicing;




    protected string message = "undifined battle action message";

    public virtual void Action(Actor target1, Actor target2, GenericBattleAction action)
    {
        //overide with action logic

    }

    public override string ToString()
    {
        return message;
    }
}
