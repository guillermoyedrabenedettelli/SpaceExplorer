using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Canvas))]
public class ConversationManager : MonoBehaviour
{
    Dialogue[] conversationSentences;
    Button[] buttonList;
    [SerializeField] float menuDeadZone = 0.5f;
    int currentButtonIndex =0;
    bool moveUp=false;
    bool moveDown = false;
    int currentDialogueIndex = 0;
    bool playerSelection = false;
    bool activeNext=false;
    void Awake()
    {
        conversationSentences = GetComponentsInChildren<Dialogue>();
        foreach(Dialogue dialogue in conversationSentences)
        {
            dialogue.gameObject.SetActive(false);
           
        }
        currentDialogueIndex = 0;
    }

    private void Start()
    {
        conversationSentences[currentDialogueIndex].gameObject.SetActive(true);
        playerSelection = (conversationSentences[currentDialogueIndex].gameObject.GetComponent<PlayerDialogue>() != null) ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeNext)
        {
            activeNext = false;
            LoadNextDialogue();
        }
    }

    void LoadNextDialogue()
    {
        if(conversationSentences[currentDialogueIndex].GetNextDialogue()!=0)
        {
            currentDialogueIndex = conversationSentences[currentDialogueIndex].GetNextDialogue();
            conversationSentences[currentDialogueIndex].gameObject.SetActive(true);
            playerSelection = (conversationSentences[currentDialogueIndex].gameObject.GetComponent<PlayerDialogue>() != null) ? true : false;
            if (playerSelection)
            {               
                buttonList = GetComponentsInChildren<Button>();
                currentButtonIndex = 0;
                EventSystem.current.SetSelectedGameObject(buttonList[currentButtonIndex].gameObject);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }     
    }

    public void LoadNextDialogue(int nextDialogueToLoad)
    {
        conversationSentences[currentDialogueIndex].gameObject.SetActive(false);
        currentDialogueIndex = nextDialogueToLoad;
        conversationSentences[currentDialogueIndex].gameObject.SetActive(true);
        playerSelection = (conversationSentences[currentDialogueIndex].gameObject.GetComponent<PlayerDialogue>() != null) ? true : false;
        if (playerSelection)
        {          
            buttonList = GetComponentsInChildren<Button>();
            currentButtonIndex = 0;
            EventSystem.current.SetSelectedGameObject(buttonList[currentButtonIndex].gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void Next(InputAction.CallbackContext context)
    {
        if (!playerSelection)
            if (context.canceled)
                activeNext =true;
    }

    public void PressCurrentButton(InputAction.CallbackContext context)
    {
        EventSystem.current.firstSelectedGameObject.SetActive(true);
        //buttonList[currentButtonIndex].onClick.Invoke();
    }
}

