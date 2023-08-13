using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] GameObject sphere;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerMovementController.currentObjetive!=null)
            sphere.transform.LookAt(PlayerMovementController.currentObjetive.transform);
    }
}
