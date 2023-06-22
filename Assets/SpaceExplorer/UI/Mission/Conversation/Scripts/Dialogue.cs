using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] protected int nextDialogue=0;

    public int GetNextDialogue()
    {
        return nextDialogue;
    }
}
