using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : damageableWithLife
{

    MovementController movementController;
    WeaponsShip shipWeapons;
    PlayerMovementController playerMovementController;
    public GameObject DualSense;
    public bool vibrate = false;
    private  void Awake()
    {
        DualSense.SetActive(false);
        baseAwake();
        movementController = GetComponent<MovementController>();
        shipWeapons = GetComponent<WeaponsShip>();
        playerMovementController = GetComponent<PlayerMovementController>();
    }


    void Start()
    {
        DualSense.SetActive(true);
        //VibrationController vibrationController = gameObject.AddComponent<VibrationController>();
    }

  
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DropeableItem>() != null)
        {
            if (other.CompareTag("TurboFuelTank"))
            {
                TurboFuelTank turboFuelTank = other.GetComponent<TurboFuelTank>();
                movementController?.chargeTurbo(turboFuelTank);
                playerMovementController?.chargeTurbo(turboFuelTank);
            }
            else if(other.CompareTag("ReparationKit"))
            {
                ReparationKit reparationKit = other.GetComponent<ReparationKit>();
                life_dead = life_dead + (life*(reparationKit.healthRecoveryPercentag / 100));
                if(life_dead>life)
                {
                    life_dead = life;
                }
                healthBar.fillAmount = life_dead / life;
                reparationKit.DestroyItem();
            }
            else if(other.CompareTag("AmmoPack"))
            {
                AmmoPack ammoPack = other.GetComponent<AmmoPack>();
                shipWeapons?.chargeAmmo(ammoPack);
            }
            else if(other.gameObject.GetComponent<MissionObject1>()!=null)
            {
                playerMovementController.UpdateCurrentMission(2);
                MissionObject1 missionObject = other.GetComponent<MissionObject1>();
                missionObject.DestroyItem();
            }

        }
        
       
       
    }
}
