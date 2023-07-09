using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravedad : MonoBehaviour
{
    public float RadioDeAtraccion = 10.5f;
    public float FuerzaDeGravedad = 10f;
    
    private void FixedUpdate()
    {
        var Player = PlayerMovementController.Player;
        if (Player == null)
        {
            return;
        }
        // Tener la direccion para el rayo de atraccion
        Vector3 distance = vector3.distance();
    }
}
