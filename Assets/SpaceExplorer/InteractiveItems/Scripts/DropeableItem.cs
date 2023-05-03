using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropeableItem : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    Transform target;
    bool isMoving = false;
    [SerializeField] int DropRate = 20;

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
    public int GetDropRate()
    {
        return DropRate;
    }
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
