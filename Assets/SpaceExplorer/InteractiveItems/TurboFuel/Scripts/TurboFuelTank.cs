using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboFuelTank : MonoBehaviour
{

    Transform target;
    bool isMoving = false;

    
    [SerializeField] float speed = 1f;
    public float turboRecoveryPercentage = 20f;
   
    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3
                .MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    public void MoveToPlayer(Transform playerTransform)
    {
        target = playerTransform;
        isMoving = true;

    }

    public void DestroyTank()
    {
        Destroy(gameObject);
    }
}
