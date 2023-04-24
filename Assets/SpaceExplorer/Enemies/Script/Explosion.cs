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
                damageable.NotifyHit(damage_explostions);
                Debug.Log("Hay IDamageable");
            }
        }
        Destroy(this);
    }
}
