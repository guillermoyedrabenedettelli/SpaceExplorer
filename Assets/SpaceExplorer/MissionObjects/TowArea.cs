using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class TowArea : MonoBehaviour
{

    [SerializeField] GameObject towItem;
    [SerializeField] GameObject towInterface;
    [SerializeField] Image towBar;
    bool towed = false;

    void Awake()
    {
        towInterface.SetActive(false);
        UpdateProgressBar(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Trigger events
    private void OnTriggerEnter(Collider other)
    {
        if (!towed)
        {
            if (other.GetComponentInChildren<PlayerMovementController>()!=null)
            {
                other.GetComponentInChildren<PlayerMovementController>().SetCanTow(true);
                other.GetComponentInChildren<PlayerMovementController>().SetTowItem(this);
                towInterface.SetActive(true);
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!towed)
        {
            if (other.GetComponentInChildren<PlayerMovementController>() != null)
            {
                other.GetComponentInChildren<PlayerMovementController>().SetCanTow(false);
                towInterface.SetActive(false);

            }
        }
        
    }

    public void SetTowed()
    {
        towed = true;
        towInterface.SetActive(false);
    }
    public GameObject GetTowItem()
    {
        return towItem;
    }

    public void UpdateProgressBar(float percentage)
    {
        towBar.fillAmount = percentage;
    }
}
