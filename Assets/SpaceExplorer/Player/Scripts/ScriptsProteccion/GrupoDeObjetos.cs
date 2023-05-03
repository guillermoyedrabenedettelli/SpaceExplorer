using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrupoDeObjetos : MonoBehaviour
{

    public GameObject shieldPrefab;
    public int numberOfShields;
    public float radius;
    private GameObject shield;
    public float rotationSpeed = 45f; // Velocidad de rotación en grados por segundo




    void Start()
    {
        Shild();
    }

    private void Update()
    {
        Quaternion shieldRotation = shield.transform.rotation;

        // Calcular la cantidad de rotación en este frame
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Rotar el escudo alrededor del eje Y
        shieldRotation *= Quaternion.Euler(0, rotationAmount, 0);

        // Asignar la nueva rotación al objeto shield
        shield.transform.rotation = shieldRotation;
    }

    private void Shild()
    { // Crear el objeto Shield y establecerlo como padre de los escudos
        shield = new GameObject("Shield");
        shield.transform.parent = transform;
        //shield.transform.position = transform.position;
        // Añadir los hijos
        for (int i = 0; i < numberOfShields; i++)
        {
            float angle = i * Mathf.PI * 2f / numberOfShields;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
            Quaternion rot = Quaternion.Euler(0f, -angle * Mathf.Rad2Deg, 0f);
            GameObject shieldChild = Instantiate(shieldPrefab, pos, rot, shield.transform);
            shieldChild.name = "ShieldChild_" + i.ToString();
        }
        Vector3 parentPosition = transform.position;

        // Asignar la posición del objeto padre al objeto shield
        shield.transform.position = parentPosition;
    }
    
}
