using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageAll_Script : MonoBehaviour
{
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] float damage_ = 10000;
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
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.NotifyHit(damage_);

            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
