using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionArea : MonoBehaviour
{

    DropeableItem dropeableItem;
    // Start is called before the first frame update
    void Awake()
    {
        dropeableItem = GetComponentInParent<DropeableItem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.transform.parent.parent = null;
            dropeableItem.MoveToPlayer(other.transform);          
        }
    }
}
