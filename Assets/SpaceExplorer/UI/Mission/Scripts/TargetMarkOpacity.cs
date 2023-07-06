using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TargetMarkOpacity : MonoBehaviour
{
    SpriteRenderer targetMark;
    [SerializeField] Transform playerTransform;
    [SerializeField] float maxDistance=5000f;
    [SerializeField] float minDistance=30f;
    void Awake()
    {
        targetMark = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceWithPlayer;   
        if (playerTransform != null)
        {
            distanceWithPlayer = Vector3.Distance(playerTransform.position, transform.position);
            targetMark.color= new Color(targetMark.color.r, targetMark.color.g, targetMark.color.b, (distanceWithPlayer > maxDistance) ? 1f : ((distanceWithPlayer < minDistance) ? 0f : ((1 - 0) * (distanceWithPlayer - minDistance) / (maxDistance - minDistance) + 0)));
        }
            
    }
}
