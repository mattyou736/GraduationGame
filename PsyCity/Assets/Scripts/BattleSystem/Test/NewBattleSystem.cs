using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState
{
    START, PLAYERTURN, ENEMYTURN, WON, LOST
}


public class NewBattleSystem : GenericWindow
{
    public BattleState state;
    public PlayerStats playerMan;

    [Header("Party")]
    public Actor[] partyMembers;
    public Transform[] partyMemberSpawn;
    public Text[] partyMemberName;
    public Text[] partyMemberHP;
    public Text[] partyMemberAP;

    public GameObject partyMembersActionsMenu;
    public GameObject partyMembersItemsMenu;

    int currentPartyMemberTurn = 0;

    [Header("Enemy")]
    public Actor[] enemys;
    public Actor enemy;
    public Transform enemySpawn;
    public Text enemyName;
    public Text enemyHP;

    public ButtonSetter action1, action2, action3;

    private ShakeManager shakeManager;
    public RectTransform partyWindowRect, enemyWindowRect;
    //public RectTransform monsterRect;

    public delegate void BattleOver(bool playerWin);
    public BattleOver battleOverCallback;


    // Start is called before the first frame update
    void OnEnable()
    {
        shakeManager = GetComponent<ShakeManager>();
        state = BattleState.START;
        SetupBattle();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            if (currentPartyMemberTurn >= 2)
            {
                currentPartyMemberTurn = 0;
            }
            else
            {
                currentPartyMemberTurn++;
            }
        }
    }

    //setting up the right actors at the right positions
    void SetupBattle()
    {
        //decides randomly based on array length what enemy to face // should probably make a if for a boss
        enemy = enemys[(int)Random.Range(0, enemys.Length)].Clone<Actor>();
        string entryMessage = "A lazy " + enemy.name + "Showed up!!!";
        DisplayMessage(entryMessage);

        //reset hp and turns
        enemy.ResetHealth();
        currentPartyMemberTurn = 0;

        //spawn player images
        Instantiate(partyMembers[0].characterObject, partyMemberSpawn[0]);
        Instantiate(partyMembers[1].characterObject, partyMemberSpawn[1]);
        Instantiate(partyMembers[2].characterObject, partyMemberSpawn[2]);

        //set the hud right
        SetHUD();
        
        //in the enemies case i should make a randomizer that picks between a few diffrent actors to have the encounter be random
        Instantiate(enemy.characterObject, enemySpawn);


        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    public void SetHUD()
    {
        for (int i = 0; i < partyMembers.Length; i++)
        {
            partyMemberName[i].text = partyMembers[i].name;
            partyMemberHP[i].text = "HP-" + partyMembers[i].health.ToString();
            partyMemberAP[i].text = "AP-" + partyMembers[i].ac.ToString();
        }

        enemyName.text = enemy.name;
        enemyHP.text = "HP-" + enemy.health.ToString();

    }

    //message at battle
    void DisplayMessage(string text)
    {
        Debug.Log(text);
        var messageWindow = manager.Open((int)Windows.MessageWindow - 1, false) as MessageWindow;
        messageWindow.text = text;
    }


    public void PlayerTurn()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        
        
        Debug.Log("player " + currentPartyMemberTurn + " turn");

    }

    public void OnActionsButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        //open menu of actions that are dependend on the characters actions in scriptable objects
        partyMembersActionsMenu.SetActive(true);

        action1.SetButtons(partyMembers,currentPartyMemberTurn);
        action2.SetButtons(partyMembers, currentPartyMemberTurn);
        action3.SetButtons(partyMembers, currentPartyMemberTurn);
    }

    public void OnItemsButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        //open menu of actions that are dependend on the characters actions in scriptable objects
        partyMembersItemsMenu.SetActive(true);

        
    }

    //what to do on attack / all are diffrent actions buttons will use this function
    //and in it we will calculate based on the scriptable object value what happens
    public void OnAttackButton(int action)
    {
        if (state != BattleState.PLAYERTURN)
            return;

        if (currentPartyMemberTurn >= 2)
        {
            currentPartyMemberTurn = 0;
        }
        else
        {
            currentPartyMemberTurn++;
        }


        StartCoroutine(PlayerAttack(partyMembers[currentPartyMemberTurn].actions[action], partyMembers[currentPartyMemberTurn],enemy));
    }

    IEnumerator PlayerAttack(GenericBattleAction action, Actor target1, Actor target2)
    {
        //calculate and deal damage 
        action.Action(target1, target2, action);//player atk monster

        //sets value in hud
        SetHUD();
        shakeManager.Shake(enemyWindowRect, .5f, 1);

        DisplayMessage(action.ToString());
        partyMembersActionsMenu.SetActive(false);

        yield return new WaitForSeconds(2f);

        // check if enemy is deade
        //change state based on what happen enemy alive = enemy turn / enemy dead = end battle
        if(enemy.health <= 0)
        {
            state = BattleState.WON;
            StartCoroutine(OnBattleOver());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyAttack());
        }
        
    }

    IEnumerator EnemyAttack()
    {
        GenericBattleAction action = enemy.actions[0];
        Actor target = partyMembers[(int)Random.Range(0, partyMembers.Length)];

        //calculate and deal damage 
        action.Action(enemy, target, action);//player atk monster

        //sets value in hud
        SetHUD();
        shakeManager.Shake(partyWindowRect, .5f, 1);

        DisplayMessage(action.ToString());
        partyMembersActionsMenu.SetActive(false);

        yield return new WaitForSeconds(2f);

        // check if enemy is deade
        //change state based on what happen enemy alive = enemy turn / enemy dead = end battle
        state = BattleState.PLAYERTURN;

    }


    IEnumerator OnBattleOver()
    {
        var message = (partyMembers[0].alive ? "The team" : enemy.name) + " has won the battle";
        var gold = Random.Range(0, enemy.gold);
        if (gold > 0 && partyMembers[0].alive)
        {
            message += " " + partyMembers[0].name + " recieves $" + gold + ".";
            partyMembers[0].IncreaseGold(gold);
            SetHUD();

        }

        yield return new WaitForSeconds(1);
        DisplayMessage(message);
        yield return new WaitForSeconds(1);
        

        if (battleOverCallback != null)
            battleOverCallback(partyMembers[0].alive);

        CloseWindow();



    }

    void CloseWindow()
    {
        this.Close();
        //playerMan.BattleOver();
    }

    public void BackOutMenu()
    {
        partyMembersActionsMenu.SetActive(false);
    }


}
