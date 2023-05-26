using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Patrol : MonoBehaviour
{

    [SerializeField] Transform[] points;
    [SerializeField] float speed=10;
    int currentpoint=0;
    // Start is called before the first frame update
    void Awake()
    {
        transform.LookAt(points[currentpoint]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, points[currentpoint].position)<1)
        {
            currentpoint = GetNextPoint(currentpoint);
            transform.LookAt(points[currentpoint]);
        }
        transform.position += speed * Time.deltaTime * transform.forward;
    }
    int GetNextPoint(int node)
    {
        node += 1;
        if (node < points.Length)
        {
            return node;
        }
        else
        {
            return 0;
        }
    }
}
