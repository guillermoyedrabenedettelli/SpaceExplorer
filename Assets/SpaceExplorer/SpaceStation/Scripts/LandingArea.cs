using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingArea : MonoBehaviour
{
    [SerializeField] Transform landingPoint;
    [SerializeField] GameObject landingCamera;
    void Awake()
    {
        if(landingCamera!=null)
        {
            landingCamera.gameObject.SetActive(false);
        }
    }

        private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerMovementController pmc = other.gameObject.GetComponent<PlayerMovementController>();
            pmc.ReadyToLand(true, landingPoint,landingCamera);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovementController pmc = other.gameObject.GetComponent<PlayerMovementController>();
            pmc.SetReadyToLand(false);
        }
    }
}

