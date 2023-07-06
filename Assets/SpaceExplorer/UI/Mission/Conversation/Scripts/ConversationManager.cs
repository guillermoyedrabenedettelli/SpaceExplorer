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
    int currentButtonIndex =0;

    int currentDialogueIndex = 0;
    bool playerSelection = false;
    bool activeNext=false;

    Misiones3 Missions;
    PlayerMovementController PMC;
    WeaponsShip Weapons;
    void Awake()
    {
        conversationSentences = GetComponentsInChildren<Dialogue>();
        foreach(Dialogue dialogue in conversationSentences)
        {
            dialogue.gameObject.SetActive(false);
           
        }
        currentDialogueIndex = 0;
        InputSystem.settings.defaultDeadzoneMax = 0.925f;
    }

    private void Start()
    {
        conversationSentences[currentDialogueIndex].gameObject.SetActive(true);
        playerSelection = (conversationSentences[currentDialogueIndex].gameObject.GetComponent<PlayerDialogue>() != null) ? true : false;
        this.gameObject.SetActive(false);
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
        else
        {
            Missions.asignaMision(Missions.GetCurrentMission() + 1);
            PMC.StartTakeOf();
            PMC.enabled = true;
            Weapons.enabled = true;
            Destroy(this.gameObject);
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
        activeNext = false;
    }

    public void Next(InputAction.CallbackContext context)
    {
        if (!playerSelection)
            if (context.performed)
                activeNext =true;
    }

    public bool GetPlayerSelection()
    {
        return playerSelection;
    }

    public void PressCurrentButton(InputAction.CallbackContext context)
    {
        EventSystem.current.firstSelectedGameObject.SetActive(true);
        //buttonList[currentButtonIndex].onClick.Invoke();
    }

    public void SetMision(GameObject player,Misiones3 m)
    {
        Missions = m;
        PMC = player.GetComponent<PlayerMovementController>();
        Weapons=player.GetComponent<WeaponsShip>();
        player.GetComponent<ConversationControl>().SetConversationManager(this);
    }
}

