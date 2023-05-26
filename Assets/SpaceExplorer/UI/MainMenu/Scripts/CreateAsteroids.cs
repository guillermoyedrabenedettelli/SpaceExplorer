using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAsteroids : MonoBehaviour
{

    [SerializeField] GameObject prefab;
    [SerializeField] int amount=20;
    [SerializeField] float radius=10f;
    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < amount; i++)
        {
            var position = new Vector3(transform.position.x + Random.Range(-radius, radius), transform.position.y, transform.position.y+Random.Range(-radius, radius));
            var asteroid = Instantiate(prefab, position, Quaternion.identity);
            asteroid.transform.parent = this.transform;
        }     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
