using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actor : ScriptableObject
{
    //name of actor
    public new string name;
    public GameObject characterObject;
    public Image characterImage;

    [Header("Stats")]
    public int health;
    public int maxHealth;
    public int speed;

    [Header("Action Points")]
    public int ac;
    public int maxAc;

    [Header("Weakness Element")]
    public bool fire;
    public bool lighting;
    public bool water;
    public bool neutral;

    [Header("Weakness Property")]
    public bool physical;
    public bool ranged;
    public bool slicing;


    public GenericBattleAction[] actions;



    //should be removed and added somewhere else-----
    //money its carrying
    public int gold;
    public Vector2 attackRange = Vector2.one;
    //-----------------------------------------------

    public bool alive
    {
        get
        {
            return health > 0;
        }
    }
    //never drop below 0
    public void DecreaseHealth(int value)
    {
        health = Mathf.Max(health - value, 0);
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    public void IncreaseGold(int value)
    {
        gold += value;
    }
    public void DecreaseGold(int value)
    {
        gold -= value;
    }

    //cloning  actor 
    public T Clone<T>() where T : Actor
    {
        var clone = ScriptableObject.CreateInstance<T>();
        clone.name = name;
        clone.characterObject = characterObject;
        clone.characterImage = characterImage;
        clone.health = health;
        clone.maxHealth = maxHealth;
        clone.speed = speed;
        clone.ac = ac;
        clone.maxAc = maxAc;

        //weaknesses
        clone.fire = fire;
        clone.lighting = lighting;
        clone.water = water;
        clone.neutral = neutral;
        clone.physical = physical;
        clone.ranged = ranged;
        clone.slicing = slicing;

        //actions
        clone.actions = actions;

        clone.gold = gold;
        clone.attackRange = attackRange;

        return clone;

    }

}
