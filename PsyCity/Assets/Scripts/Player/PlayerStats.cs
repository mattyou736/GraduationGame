using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 5;
    public int EXP = 40;
    public int gold = 1000;
    public bool togglePlayerMovement;

    public Quest quest;
    //public GameObject battleWindow;

    public BattleWindow battleWindow;
    //mesage window
    public WindowManager windowManager
    {
        get
        {
            return GenericWindow.manager;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q pressed");
            GoBattle();
        }
    }

    public void GoBattle()
    {
        //battleWindow.SetActive(true);
        battleWindow = windowManager.Open((int)Windows.BattleScreen - 1, false) as BattleWindow;
        //battleWindow.battleOverCallback += BattleOver;
        //battleWindow.StartBattle(playerActor, monsterActor);
        togglePlayerMovement = false;
    }

    public void BattleOver()
    {
        Debug.Log("closing window");
        battleWindow.Close();
        togglePlayerMovement = true;
    }
}
