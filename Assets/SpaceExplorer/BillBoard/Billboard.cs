using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public GameObject billboardPlane;

    void Update()
    {
        billboardPlane.transform.LookAt(Camera.main.transform);
    }
}


