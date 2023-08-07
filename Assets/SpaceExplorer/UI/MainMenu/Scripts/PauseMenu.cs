using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class PauseMenu : MonoBehaviour
{

    Button firstButton;
    [SerializeField] GameObject pauseMenu;
    WeaponsShip WeaponsController;
    PlayerMovementController PlayerController;
    void Awake()
    {
        firstButton=GetComponentInChildren<Button>();
        if(pauseMenu!=null)
            pauseMenu.gameObject.SetActive(false);
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

    public void SetRequeriments(WeaponsShip Weapons, PlayerMovementController pmc)
    {
        WeaponsController = Weapons;
        PlayerController = pmc;
    }

    public void PauseGame()
    {
        PlayerController.enabled = false;
        WeaponsController.enabled = false;
        pauseMenu.gameObject.SetActive(true);
        SetFirstOption();
        Time.timeScale = 0.0f;
    }


    public void ReturnToCheckPoint()
    {
        PlayerController.gameObject.transform.position= Checkpoint.checkpointPosition;
        PlayerController.gameObject.GetComponentInChildren<PlayerDamageable>().fullHeal();
        PlayerController.FullChargeTurbo();
        PlayerController.enabled = true;
        WeaponsController.enabled = true;
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1.0f;

    }
}
