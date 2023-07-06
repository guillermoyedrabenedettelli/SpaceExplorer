using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class MenuControls : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(this.GetComponentInChildren<Button>().gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PressCurrentButton(InputAction.CallbackContext context)
    {
        EventSystem.current.firstSelectedGameObject.SetActive(true);
        //buttonList[currentButtonIndex].onClick.Invoke();  
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
