using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoAsteroide : MonoBehaviour
{
    [SerializeField]
    private float Giro;

    public float amplitude = 1f;
    public float frequency = 1f;
    public float speed = 1f;
    public float rotationSpeed = 1f;

    private float startY;
    private float startZ;
    private float startX;
    private Vector3 direction;

    private void Start()
    {
        startY = transform.position.y;
        startZ = transform.position.z;
        startX = transform.position.x;
        direction = Random.insideUnitSphere.normalized; // Obtiene una dirección aleatoria normalizada
        //GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Giro;
    }

    private void Update()
    {
        // Calcula la nueva posición del objeto en función del tiempo y de la dirección aleatoria
        float newY = startY + amplitude * Mathf.Sin(frequency * Time.time * speed);
        float newZ = startZ + amplitude * Mathf.Cos(frequency * Time.time * speed);
        float newX = startX + amplitude * Mathf.Sin(frequency * Time.time * speed) + Mathf.Cos(frequency * Time.time * speed);
        Vector3 newPos = transform.position + direction * Time.deltaTime * speed;
        transform.position = new Vector3(newPos.x, newY, newPos.z);

        // Rota el objeto en su eje
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
        
    }
}
