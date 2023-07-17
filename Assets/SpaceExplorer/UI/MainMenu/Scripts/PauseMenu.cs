using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    Button firstButton;
    [SerializeField] GameObject pauseMenu;
    WeaponsShip WeaponsController;
    PlayerMovementController PlayerController;
    void Awake()
    {
        firstButton=GetComponentInChildren<Button>();
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(string name)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(name);
    }

    public void SetFirstOption()
    {
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }

    public void UnPauseGame()
    {
        PlayerController.enabled = true;
        WeaponsController.enabled = true;
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void PauseGame(WeaponsShip Weapons, PlayerMovementController pmc)
    {
        WeaponsController = Weapons;
        PlayerController = pmc;

        pmc.enabled = false;
        Weapons.enabled = false;
        pauseMenu.gameObject.SetActive(true);
        SetFirstOption();
        Time.timeScale = 0.0f;
    }
}
