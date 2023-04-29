using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionArea : MonoBehaviour
{

    TurboFuelTank turboFuelTank;
    // Start is called before the first frame update
    void Awake()
    {
        turboFuelTank = GetComponentInParent<TurboFuelTank>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            turboFuelTank.MoveToPlayer(other.transform);
        }
    }
}
