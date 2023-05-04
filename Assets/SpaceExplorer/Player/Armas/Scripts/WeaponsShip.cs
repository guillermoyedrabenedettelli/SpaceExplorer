using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    [SerializeField] float bulletVelocity_ametralladora = 500f;
    [SerializeField] float bulletVelocity_lanzamisiles = 500f;
    [SerializeField] float bulletVelocity_energia = 1000f;
    private float timeDestroy;
    private float recoil;
    private bool puedoDisparar = true;

    [Header("Ammo")]
    [SerializeField] int muniAmetralladora = 100;
    [SerializeField] int muniLanzagranadas = 50;
    [SerializeField] float maxEnergiaDisparo = 100f;
    private float energiaDisparo;

    [Header("UI")]
    [SerializeField] TMP_Text weaponsAmmo;
    [SerializeField] Image laserInterface;
    Image laserTemperature;

    [Header("Input Shoot")]
    [SerializeField] InputAction ShootInput;
    [SerializeField] InputAction ReloadInput;
    private void Awake()
    {
        ShootInput.Enable();
        ReloadInput.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {

        bulletReference = bulletReferenceAmetralladora;
        bulletPrefab = bulletPrefabAmetralladora;

        armaUsada = "ametralladora";
        bulletVelocity = 1000.0f;
        timeDestroy = 500.0f;
        recoil = 0.1f;
        UpdateAmmo(muniAmetralladora);
        if (laserInterface != null)
        {
            laserTemperature = laserInterface.GetComponentsInChildren<Image>()[1];
            laserTemperature.fillAmount = 0f;
            HideLaserInterface();
        }
        energiaDisparo = maxEnergiaDisparo;
    }

    // Update is called once per frame
    void Update()
    {
        if (puedoDisparar)
        {
            if (ShootInput.IsPressed())
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
            }
            else
            {
                if (energiaDisparo != maxEnergiaDisparo)
                {
                    StartCoroutine(RecargaEnergia());
                }
            }
        }
        if (ReloadInput.WasPressedThisFrame())
        {
            StopAllCoroutines();
            changeWeapon();
        }
        //Debug.Log("Ametralladora: " + muniAmetralladora);
        //Debug.Log("Lanzamisiles: " + muniLanzagranadas);
        //Debug.Log("Energía: " + energiaDisparo);
    }
    bool checkTengoMuni()
    {
        if (armaUsada == "ametralladora")
        {
            if (muniAmetralladora > 0)
            {
                return true;
            }
            else
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
            UpdateAmmo(muniLanzagranadas);
            bulletVelocity = bulletVelocity_lanzamisiles;
            timeDestroy = 500;
            recoil = 0.5f;
        }
        else if (armaUsada == "lanzamisiles")
        {
            bulletPrefab = bulletPrefabEnergia;
            bulletReference = bulletReferenceEnergia;

            armaUsada = "energia";
            ShowLaserInterface();
            bulletVelocity = bulletVelocity_energia;
            timeDestroy = 500.0f;
            recoil = 0.05f;
        }
        else if (armaUsada == "energia")
        {
            bulletPrefab = bulletPrefabAmetralladora;
            bulletReference = bulletReferenceAmetralladora;

            armaUsada = "ametralladora";
            HideLaserInterface();
            UpdateAmmo(muniAmetralladora);
            bulletVelocity = bulletVelocity_ametralladora;
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
        if (energiaDisparo > maxEnergiaDisparo)
        {
            energiaDisparo = maxEnergiaDisparo;
        }
        UpdateTemperature();


    }

    IEnumerator Recoil()
    {
        puedoDisparar = false;
        if (armaUsada == "ametralladora")
        {
            muniAmetralladora--;
            UpdateAmmo(muniAmetralladora);
        }
        else if (armaUsada == "lanzamisiles")
        {
            muniLanzagranadas--;
            UpdateAmmo(muniLanzagranadas);
        }
        else if (armaUsada == "energia")
        {
            energiaDisparo = energiaDisparo - 1.0f;
            UpdateTemperature();
        }
        yield return new WaitForSecondsRealtime(recoil);
        puedoDisparar = true;
    }

    void UpdateAmmo(int ammo)
    {
        if (weaponsAmmo != null)
        {
            weaponsAmmo.text = ammo.ToString();
        }
    }
    void UpdateTemperature()
    {
        if (laserTemperature != null)
        {
            float fillAmount = (maxEnergiaDisparo - energiaDisparo) / maxEnergiaDisparo;
            laserTemperature.fillAmount = fillAmount;
        }
    }
    void ShowLaserInterface()
    {
        if (weaponsAmmo != null)
        {
            weaponsAmmo.gameObject.SetActive(false);
        }
        if (laserInterface != null)
        {
            laserInterface.gameObject.SetActive(true);
        }
    }
    void HideLaserInterface()
    {
        if (weaponsAmmo != null)
        {
            weaponsAmmo.gameObject.SetActive(true);
        }
        if (laserInterface != null)
        {
            laserInterface.gameObject.SetActive(false);
        }
    }
    public void chargeAmmo(AmmoPack ammoPack)
    {
        int ammo = 0;
        if(ammoPack.ammoType=="ametralladora")
        {
            muniAmetralladora += ammoPack.ammoToRecover;
            ammo = muniAmetralladora;
        }
        if(ammoPack.ammoType== "lanzamisiles")
        {
            muniLanzagranadas += ammoPack.ammoToRecover;
            ammo = muniLanzagranadas;
        }
        if (ammoPack.ammoType == armaUsada)
        {
            UpdateAmmo(ammo);
        }
        ammoPack.DestroyItem();
    }
}
