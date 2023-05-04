using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : damageableWithLife
{

    MovementController movementController;
    WeaponsShip shipWeapons;

    private  void Awake()
    {
        baseAwake();
        movementController = GetComponent<MovementController>();
        shipWeapons = GetComponent<WeaponsShip>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DropeableItem>() != null)
        {
            if (other.CompareTag("TurboFuelTank"))
            {
                TurboFuelTank turboFuelTank = other.GetComponent<TurboFuelTank>();
                movementController?.chargeTurbo(turboFuelTank);
            }
            if(other.CompareTag("ReparationKit"))
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
            if(other.CompareTag("AmmoPack"))
            {
                AmmoPack ammoPack = other.GetComponent<AmmoPack>();
                shipWeapons?.chargeAmmo(ammoPack);
            }
        }
        
       
       
    }
}
