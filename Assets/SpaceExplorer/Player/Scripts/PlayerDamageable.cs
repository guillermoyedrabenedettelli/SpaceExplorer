using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class PlayerDamageable : damageableWithLife
{
    [Header("Vibration")]
    [SerializeField] float vibrationDuration = 1f;
    [SerializeField] float vibrationIntensity = 0.5f;

    public GameObject DualSense;
    public bool vibrate = false;

    private Gamepad gamepad;
    private bool AlertLife = false;

    MovementController movementController;
    WeaponsShip shipWeapons;
    VibrationController vibrationContrller;
    PlayerMovementController playerMovementController;
    AnimationCurve vibrationCurve;

    [Header("Materials")]
    [SerializeField] Renderer tanksToChange;
    [SerializeField] Renderer cuerpoToChange;
    [SerializeField] Material HealthyMaterial;
    [SerializeField] Material DangerMaterial;
    Material[] dangerTanks;
    Material[] healthyTanks;
    Material[] dangerBody;
    Material[] healthyBody;


    private void Awake()
    {
        vibrationCurve = new AnimationCurve();
        // Agregamos puntos clave (keyframes) al curva
        vibrationCurve.AddKey(0f, 0f); // En el tiempo 0, el valor es 0
        vibrationCurve.AddKey(1f, 0.5f); // En el tiempo 1, el valor es 1
        vibrationCurve.AddKey(2f, 1f); // En el tiempo 2, el valor es 0.
        vibrationCurve.SmoothTangents(1, 0f);
        if(DualSense!=null)
            DualSense.SetActive(false);
        baseAwake();
        movementController = GetComponent<MovementController>();
        vibrationContrller = GetComponent<VibrationController>();
        shipWeapons = GetComponent<WeaponsShip>();
        playerMovementController = GetComponent<PlayerMovementController>();


        dangerTanks = tanksToChange.materials;
        dangerTanks[2] = DangerMaterial;
        healthyTanks = tanksToChange.materials;
        healthyTanks[2] = HealthyMaterial;

        dangerBody = cuerpoToChange.materials;
        dangerBody[1] = DangerMaterial;
        healthyBody = cuerpoToChange.materials;
        healthyBody[1] = HealthyMaterial;




    }


    void Start()
    {
        if(DualSense!=null)
            DualSense.SetActive(true);
        //VibrationController vibrationController = gameObject.AddComponent<VibrationController>();
    }

    void Update()
    {
        if (timeCurrentSetMotor >= 1)
        {
            timeCurrentSetMotor -= Time.deltaTime;
            Gamepad.current?.SetMotorSpeeds(0.25f, 0.75f);
            //this.gameObject.GetComponent<CameraShaker>().StartShake(1, .1f);
        }
        else
        {
            timeCurrentSetMotor = 0;
            Gamepad.current?.SetMotorSpeeds(0f, 0f);
        }

        if (life_dead < (life / 2))
        {
            vibrationContrller.VibrationSense(VibrationSense.Spiral, true);
            if (tanksToChange.materials!= dangerTanks)
            {
                tanksToChange.materials = dangerTanks;
                cuerpoToChange.materials = dangerBody;
            }
            Debug.Log("Spiral");
            AlertLife = true;
        }
        else
        {
            if (AlertLife)
            {
                vibrationContrller.Active = false;
                AlertLife = false;
                Debug.Log("Pausa");
            }
        }
    }



    private IEnumerator spiral()
    {
        float startTime = Time.time;

        float elapsedTime = Time.time - startTime;
        float normalizedTime = Mathf.Clamp01(elapsedTime / vibrationDuration);
        float vibrationValue = vibrationCurve.Evaluate(normalizedTime) * vibrationIntensity;

        Vector2 normalizedPosition = new Vector2(Mathf.Sin(elapsedTime * 3f), Mathf.Cos(elapsedTime * 3f)).normalized;
        gamepad.SetMotorSpeeds(vibrationValue * normalizedPosition.x, vibrationValue * normalizedPosition.y);
        yield return null;

        gamepad.ResetHaptics();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DropeableItem>() != null)
        {
            if (other.CompareTag("TurboFuelTank"))
            {
                TurboFuelTank turboFuelTank = other.GetComponent<TurboFuelTank>();
                movementController?.chargeTurbo(turboFuelTank);
                playerMovementController?.chargeTurbo(turboFuelTank);
            }
            else if (other.CompareTag("ReparationKit"))
            {
                ReparationKit reparationKit = other.GetComponent<ReparationKit>();
                life_dead = life_dead + (life * (reparationKit.healthRecoveryPercentag / 100));
                if (life_dead > (life / 2))
                {
                    if (tanksToChange.materials != healthyTanks)
                    {
                        tanksToChange.materials = healthyTanks;
                        cuerpoToChange.materials = healthyBody;
                    }
                }
                if (life_dead > life)
                {
                    life_dead = life;
                }
                healthBar.fillAmount = life_dead / life;
                reparationKit.DestroyItem();
            }
            else if (other.CompareTag("AmmoPack"))
            {
                AmmoPack ammoPack = other.GetComponent<AmmoPack>();
                shipWeapons?.chargeAmmo(ammoPack);
            }
            else if (other.gameObject.GetComponent<MissionObject1>() != null)
            {
                playerMovementController.UpdateCurrentMission(2);
                MissionObject1 missionObject = other.GetComponent<MissionObject1>();
                missionObject.DestroyItem();
            }

        }
    }

    public void fullHeal()
    {
        life_dead = life;
        healthBar.fillAmount = 1;
        if (tanksToChange.materials != healthyTanks)
        {
            tanksToChange.materials = healthyTanks;
            cuerpoToChange.materials = healthyBody;
        }

    }


    protected override void onDamageableDies()
    {
        Gamepad.current?.SetMotorSpeeds(0f, 0f);
        playerMovementController.enabled = false;
        alreadyDead = true;
        onDeath.Invoke();
    }
}
