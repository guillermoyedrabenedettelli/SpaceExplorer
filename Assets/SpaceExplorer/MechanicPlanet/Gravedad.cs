using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravedad : MonoBehaviour
{
   public float RadioDeAtraccion = 10.5f;  // Radio de atracción
    public float FuerzaDeGravedad = 10f;  // Fuerza de gravedad

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, RadioDeAtraccion);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                Vector3 direction = transform.position - collider.transform.position;
                collider.GetComponent<Rigidbody>().AddForce(direction.normalized * FuerzaDeGravedad);
            }
        }
    }
}
