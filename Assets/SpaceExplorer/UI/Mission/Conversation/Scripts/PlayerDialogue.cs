using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogue : Dialogue
{


    public void OnSelectOption(int newNextDialogue)
    {
        nextDialogue = newNextDialogue;
    }
}


