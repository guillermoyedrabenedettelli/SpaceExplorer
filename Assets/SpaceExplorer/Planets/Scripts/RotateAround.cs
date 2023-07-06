using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] GameObject rotateTarget;
    [SerializeField] float gradesPerSecond=20f;
    void Update()
    {
        transform.RotateAround(rotateTarget.transform.position, Vector3.up, gradesPerSecond * Time.deltaTime);
    }
}
