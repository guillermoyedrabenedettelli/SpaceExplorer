using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstInterference : MonoBehaviour
{
    [SerializeField] float positionToActiveConversation=0f;
    [SerializeField] GameObject ConversationToActive;

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.x < positionToActiveConversation + 30f && gameObject.transform.position.x > positionToActiveConversation - 30f && ConversationToActive!=null)
        {
            ConversationToActive.SetActive(true);
            Destroy(this);
        }
    }
}
