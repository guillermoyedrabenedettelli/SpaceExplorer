using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectChild : MonoBehaviour
{
    public GameObject padre;
    private void Update()
    {
        // Verificar si el objeto padre tiene hijos
        if (padre.transform.childCount == 0)
        {
            Debug.Log("El objeto "+ this.gameObject.name+" no tiene hijos");
            // Hacer algo si el objeto padre no tiene hijos
            Destroy(padre);
        }
        else
        {
            // El objeto padre tiene hijos
        }
    }

}
