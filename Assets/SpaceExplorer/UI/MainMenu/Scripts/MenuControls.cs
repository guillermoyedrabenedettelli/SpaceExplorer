using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class MenuControls : MonoBehaviour
{
    [SerializeField] float menuDeadZone = 0.5f;
    Button[] buttonList;
    int currentButtonIndex=0;
    bool moveUp=false;
    bool moveDown = false;


    // Start is called before the first frame update
    void Awake()
    {
        buttonList=GetComponentsInChildren<Button>();
        currentButtonIndex = 0;
        buttonList[currentButtonIndex].Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveDown)
        {
            currentButtonIndex = (currentButtonIndex + 1 < buttonList.Length) ? currentButtonIndex + 1 : 0;
            buttonList[currentButtonIndex].Select();
            moveDown = false;
            moveUp = false;
        }
        else if (moveUp)
        {
            currentButtonIndex = (currentButtonIndex - 1 < 0) ? buttonList.Length - 1 : currentButtonIndex - 1;
            buttonList[currentButtonIndex].Select();
            moveUp = false;
        }
    }

    public void NextButton(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() >= menuDeadZone)
            moveDown = true;    
    }
    public void PreviousButton(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() >= menuDeadZone)     
            moveUp = true;
    }
    public void PressCurrentButton(InputAction.CallbackContext context)
    {
      buttonList[currentButtonIndex].onClick.Invoke();  
    }
}
