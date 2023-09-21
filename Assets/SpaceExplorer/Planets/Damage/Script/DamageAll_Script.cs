using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAll_Script : MonoBehaviour
{
    [SerializeField] SphereCollider sphereCollider;
    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();   
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject);
        if (collision != null)
        {
            Destroy(collision.gameObject);   
        }
    }
}
