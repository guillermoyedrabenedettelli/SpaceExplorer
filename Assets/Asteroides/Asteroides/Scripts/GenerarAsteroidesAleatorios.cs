using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerarAsteroidesAleatorios : MonoBehaviour
{
    public GameObject esferaPrefab;
    public int cantidad = 10;
    public float margen = 100f;

    //Rotacion senoidal
    public float amplitud = 10f;
    public float velocidad = 1f;

    private float tiempo;

    public bool TrigometriaBehavior = false;
    public bool[] Behavior = { false, false, false }; 
    private void Start()
    {
        for (int i = 0; i < cantidad; i++)
        {
            // Generar una posici�n aleatoria dentro del margen
            Vector3 posicion = new Vector3(
                Random.Range(-margen, margen),
                Random.Range(-margen, margen),
                Random.Range(-margen, margen)
            );

            // Crear la esfera en la posici�n aleatoria generada
            GameObject esfera = Instantiate(esferaPrefab, posicion, Quaternion.identity, this.gameObject.transform);

        }
        int cantidadHijos = this.transform.childCount;
        Debug.Log("El objeto padre tiene " + cantidadHijos + " hijos.");
    }
    private void Update()
    {
        // Actualizar el tiempo en cada frame
        
        if (TrigometriaBehavior)
        {
            tiempo += Time.deltaTime * 4;
            // Calcular la rotaci�n utilizando la funci�n seno
            float rotacion = Mathf.Sin(tiempo) * amplitud;
            float rotacionCos = Mathf.Cos(tiempo) * amplitud;

            // Aplicar la rotaci�n al Game Object
            transform.rotation = Quaternion.Euler(rotacion, tiempo, rotacionCos);
        }
        else
        {
            tiempo += Time.deltaTime * velocidad;
            //this.transform.rotation = Quaternion.Euler(tiempo, tiempo, tiempo);
            if (Behavior[0] == true)
            {
                Behavior[1] = false;
                this.transform.rotation = Quaternion.Euler(0, tiempo, 0);
            }
            if(Behavior[1] == true)
            {
                Behavior[0] = false;
                this.transform.rotation = Quaternion.Euler(0, tiempo, tiempo);
            }
        }
        //int cantidadHijos = this.transform.childCount;
        //Debug.Log("El objeto padre tiene " + cantidadHijos + " hijos.");
    }
    
}
