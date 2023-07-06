using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConversationControl : MonoBehaviour
{
    ConversationManager CM;
    void Awake()
    {
    }

    public void Next(InputAction.CallbackContext context)
    {
        if (CM != null && CM.isActiveAndEnabled && !CM.GetPlayerSelection())
        {
            CM.Next(context);
        }  
    }

    public void SetConversationManager(ConversationManager cm)
    {
        CM=cm;
    }
}
