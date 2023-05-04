using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public float intensity = 0.2f;  // intensidad de la vibración
    public float speed = 1.0f;  // velocidad de la vibración

    private Vector3 originalPosition;  // posición original de la cámara

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        // Genera valores aleatorios utilizando PerlinNoise
        float x = Mathf.PerlinNoise(0.0f, Time.time * speed) * 2.0f - 1.0f;
        float y = Mathf.PerlinNoise(Time.time * speed, 0.0f) * 2.0f - 1.0f;

        // Aplica la vibración a la posición original de la cámara
        Vector3 offset = new Vector3(x, y, 0.0f) * intensity;
        transform.position = originalPosition + offset;
    }
}
