using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public float intensity = 0.2f;  // intensidad de la vibraci�n
    public float speed = 1.0f;  // velocidad de la vibraci�n

    private Vector3 originalPosition;  // posici�n original de la c�mara

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        // Genera valores aleatorios utilizando PerlinNoise
        float x = Mathf.PerlinNoise(0.0f, Time.time * speed) * 2.0f - 1.0f;
        float y = Mathf.PerlinNoise(Time.time * speed, 0.0f) * 2.0f - 1.0f;

        // Aplica la vibraci�n a la posici�n original de la c�mara
        Vector3 offset = new Vector3(x, y, 0.0f) * intensity;
        transform.position = originalPosition + offset;
    }
}
