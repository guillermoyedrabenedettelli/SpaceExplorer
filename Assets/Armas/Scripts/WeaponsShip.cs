using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsShip : MonoBehaviour
{
    [Header("Punto de salida de las balas")]
    public GameObject bulletReferenceAmetralladora;
    public GameObject bulletReferenceLanzagranadas;
    private GameObject bulletReference;
    [Header("Balas a salir")]
    public GameObject bulletPrefabAmetralladora;
    public GameObject bulletPrefabLanzagrandas;
    private GameObject bulletPrefab;

    private string armaUsada;
    private float bulletVelocity;
    private float timeDestroy;
    private float recoil;
    private bool puedoDisparar = true;
    // Start is called before the first frame update
    void Start()
    {
        
        bulletReference = bulletReferenceAmetralladora;
        bulletPrefab = bulletPrefabAmetralladora;

        armaUsada = "ametralladora";
        bulletVelocity = 1000.0f;
        timeDestroy = 500.0f;
        recoil = 0.1f;
}

    // Update is called once per frame
    void Update()
    {
        if (puedoDisparar)
        {
            if (Input.GetButton("Fire1"))
            {
                GameObject bulletTemp = Instantiate(bulletPrefab, bulletReference.transform.position, bulletReference.transform.rotation);
                Rigidbody RB = bulletTemp.GetComponent<Rigidbody>();
                RB.AddForce(bulletTemp.transform.forward * bulletVelocity); //ESTABLECES FUERZA, OBJETOS MÁS PESADOS ACELERARÁN MÁS LENTO, MÁS REALISTA
                                                                            //RB.velocity = bulletTemp.transform.forward * bulletVelocity; ESTABLECE VELOCIDAD BIEN SI CONOCES DISTANCIAS
                Destroy(bulletTemp, timeDestroy * Time.deltaTime);
                StartCoroutine(Recoil());
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StopAllCoroutines();
            changeWeapon();
        }
    }

    void changeWeapon()
    {
        puedoDisparar = false;
        if (armaUsada == "ametralladora")
        {
            bulletPrefab = bulletPrefabLanzagrandas;
            bulletReference = bulletReferenceLanzagranadas;

            armaUsada = "lanzamisiles";
            bulletVelocity = 500.0f;
            timeDestroy = 500.0f;
            recoil = 0.5f;
        } else if (armaUsada == "lanzamisiles")
        {
            bulletPrefab = bulletPrefabAmetralladora;
            bulletReference = bulletReferenceAmetralladora;

            armaUsada = "ametralladora";
            bulletVelocity = 1000.0f;
            timeDestroy = 500.0f;
            recoil = 0.1f;
        }
        puedoDisparar = true;
    }

    IEnumerator Recoil()
    {
        puedoDisparar = false;
        yield return new WaitForSecondsRealtime(recoil);
        puedoDisparar = true;
    }
}
