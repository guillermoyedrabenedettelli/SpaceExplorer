using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnArea : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] enemiesToActive;
   


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMovementController>()!=null)
        {
            foreach(GameObject enemy in enemiesToActive)
            {
                enemy.SetActive(true);
            }
        }
    }
}
