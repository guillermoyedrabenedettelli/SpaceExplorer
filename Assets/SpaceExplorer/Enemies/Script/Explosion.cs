using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float radius = 5f;
    [SerializeField] float damage_explostions = 20;
    [SerializeField] public LayerMask layerMask = Physics.DefaultRaycastLayers;
    void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layerMask);
        foreach (Collider c in colliders)
        {
            IDamageable damageable = c.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Debug.Log("Hay IDamageable");
                RaycastHit hit;
                if (Physics.Raycast(transform.position,
                 c.transform.position - transform.position,
                 out hit,
                 radius,
                 layerMask))
                {
                    if (hit.collider == c)
                    {
                        damageable.NotifyHit(damage_explostions);
                    }
                }
            }
        }
        Destroy(this);
    }
}
