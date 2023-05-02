using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsShip : MonoBehaviour
{
    [Header("Punto de salida de las balas")]
    public GameObject bulletReferenceAmetralladora;
    public GameObject bulletReferenceLanzagranadas;
    public GameObject bulletReferenceEnergia;
    private GameObject bulletReference;
    [Header("Balas a salir")]
    public GameObject bulletPrefabAmetralladora;
    public GameObject bulletPrefabLanzagrandas;
    public GameObject bulletPrefabEnergia;
    private GameObject bulletPrefab;

    private string armaUsada;
    private float bulletVelocity;
    private float timeDestroy;
    private float recoil;
    private bool puedoDisparar = true;

    private int muniAmetralladora = 100;
    private int muniLanzagranadas = 50;
    private float energiaDisparo = 1000.0f;
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
                bool siTengo = checkTengoMuni();
                if (siTengo)
                {
                    GameObject bulletTemp = Instantiate(bulletPrefab, bulletReference.transform.position, bulletReference.transform.rotation);
                    Rigidbody RB = bulletTemp.GetComponent<Rigidbody>();
                    RB.AddForce(bulletTemp.transform.forward * bulletVelocity); //ESTABLECES FUERZA, OBJETOS MÁS PESADOS ACELERARÁN MÁS LENTO, MÁS REALISTA
                                                                                //RB.velocity = bulletTemp.transform.forward * bulletVelocity; ESTABLECE VELOCIDAD BIEN SI CONOCES DISTANCIAS
                    Destroy(bulletTemp, timeDestroy * Time.deltaTime);
                    StartCoroutine(Recoil());
                }
            } else
            {
                if (energiaDisparo != 1000.0f)
                {
                    StartCoroutine(RecargaEnergia());
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StopAllCoroutines();
            changeWeapon();
        }
        Debug.Log("Ametralladora: " + muniAmetralladora);
        Debug.Log("Lanzamisiles: " + muniLanzagranadas);
        Debug.Log("Energía: " + energiaDisparo);
    }
    bool checkTengoMuni()
    {
        if (armaUsada == "ametralladora")
        {
            if (muniAmetralladora > 0)
            {
                return true;
            } else
            {
                return false;
            }
        }
        else if (armaUsada == "lanzamisiles")
        {
            if (muniLanzagranadas > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (armaUsada == "energia")
        {
            if (energiaDisparo > 0.0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
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
            bulletPrefab = bulletPrefabEnergia;
            bulletReference = bulletReferenceEnergia;

            armaUsada = "energia";
            bulletVelocity = 1000.0f;
            timeDestroy = 500.0f;
            recoil = 0.05f;
        } else if (armaUsada == "energia")
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

    void actualizaMuni(int cantidad, int muni)
    {
        switch (muni)
        {
            case 1:
                muniAmetralladora = muniAmetralladora + cantidad;
                break;
            case 2:
                muniLanzagranadas = muniLanzagranadas + cantidad;
                break;
        }
    }

    IEnumerator RecargaEnergia()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        energiaDisparo = energiaDisparo + 0.5f;
        if (energiaDisparo > 1000.0f)
        {
            energiaDisparo = 1000.0f;
        }
    }

    IEnumerator Recoil()
    {
        puedoDisparar = false;
        if (armaUsada == "ametralladora")
        {
            muniAmetralladora--;
        } else if (armaUsada == "lanzamisiles")
        {
            muniLanzagranadas--;
        } else if (armaUsada == "energia")
        {
            energiaDisparo = energiaDisparo - 1.0f;
        }
        yield return new WaitForSecondsRealtime(recoil);
        puedoDisparar = true;
    }
}
