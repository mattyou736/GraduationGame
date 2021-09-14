using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSetter : MonoBehaviour
{
    public int action;
    Text buttonText;
    Actor[] testparty;

    //get the name of the actions from the current party memebers turn and apply to button
    public void SetButtons(Actor[] partyMembers, int currentPartyMemberTurn)
    {
        testparty = partyMembers;
        buttonText = GetComponentInChildren<Text>();
        buttonText.text = partyMembers[currentPartyMemberTurn].actions[action].name;
    }
}
