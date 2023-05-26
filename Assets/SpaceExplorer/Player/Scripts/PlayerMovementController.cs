using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] float turboRotationLimitation = 0.5f;
    private float rotationLimit=1f;
    [SerializeField] float stopThreshold = 0.2f;

    [Header("Roll")]
    [SerializeField] float rollSpeed = 4f;
    [SerializeField] float actualRollSpeed = 0f;
    [SerializeField] float maxRollSpeed = 4f;
    [SerializeField] float rollDeceleration = 4f;


    [Header("Yaw")]
    [SerializeField] float yawSpeed = 4f;
    [SerializeField] float actualYawSpeed = 0f;
    [SerializeField] float maxYawSpeed = 4f;
    [SerializeField] float yawDeceleration = 4f;

    [Header("Pitch")]
    [SerializeField] float pitchSpeed = 4f;
    [SerializeField] float actualPitchSpeed = 0f;
    [SerializeField] float maxPitchSpeed = 4f;
    [SerializeField] float pitchDeceleration = 4f;

    [Header("Movement & Turbo")]
    [SerializeField] float defaultSpeed=2f;
    [SerializeField] float turboMaxSpeed = 40f;
    [SerializeField] float actualSpeed = 2f;
    [SerializeField] float turboAceleration = 0.5f;
    [SerializeField] float turboDeceleration = 0.5f;
    [SerializeField] float maxTurbo = 100f;
    [SerializeField] float actualTurbo = 0;
    [SerializeField] float turboReductionPerSecond =3f;
    [SerializeField] Image turboBar;
    [SerializeField] float turboRecovery = 2f;
    [SerializeField] float timeToStartRecovering = 2f;
    float actualTimeWaitingForRecover = 0f;

    [Header("Particles")]
    [SerializeField] float maxParticleSpeed=25;
    [SerializeField] float minParticleSpeed=10;
    [SerializeField] float maxParticleEmision=20f;
    [SerializeField] float minParticleEmision=10f;
    [SerializeField] ParticleSystem speedParticles;


    [Header("Camera")]
    [SerializeField] GameObject Cabina;
    [SerializeField] GameObject ShipPS4;
    bool camaraChange = false;
    [SerializeField] float maxAimHorizontalMovement = 2f;
    [SerializeField] float maxAimVerticalMovement = 1f;


    Rigidbody rigidBody;
    CharacterController ch;
    PlayerMoveCameraCentre playerMoveCameraCentre;

    private float movement;
    private float roll;
    private Vector2 yawPicth;

    void Awake()
    {
        roll = 0f;
        movement = 0f;
        yawPicth = new Vector2(0, 0);
        CabinaBool(false);
        actualTurbo = maxTurbo;
        ch = GetComponent<CharacterController>();
        playerMoveCameraCentre = GetComponentInChildren<PlayerMoveCameraCentre>();
    }

    void Update()
    {
        UpdateLocation();
        UpdateSpeedParticles();
        UpdateRotation();
    }

    void UpdateLocation()
    {
        if(movement>0 && actualTurbo>0 )
        {
            actualTurbo = actualTurbo - turboReductionPerSecond * Time.deltaTime;
            turboBar.fillAmount = actualTurbo / maxTurbo;

            if (actualTurbo < 0)
                actualTurbo = 0;
            actualSpeed = ((actualSpeed < turboMaxSpeed ? (movement * turboAceleration) + actualSpeed : turboMaxSpeed));
            actualTimeWaitingForRecover = 0f;
        }
        else
        {
            if (actualTimeWaitingForRecover >= timeToStartRecovering)
            {
                if (actualTurbo < maxTurbo)
                {
                    actualTurbo += turboRecovery * Time.deltaTime;
                    if (actualTurbo >= maxTurbo)
                    {
                        actualTurbo = maxTurbo;
                    }
                    turboBar.fillAmount = actualTurbo / maxTurbo;
                }
            }
            else
            {
                actualTimeWaitingForRecover += Time.deltaTime;
            }

            actualSpeed =(actualSpeed > defaultSpeed) ? actualSpeed - turboDeceleration : defaultSpeed;
        }



        ch.Move(-transform.forward * actualSpeed * Time.deltaTime);
        //transform.position += -transform.forward * actualSpeed * Time.deltaTime;

        

    }
    void UpdateSpeedParticles()
    {
        if (speedParticles != null)
        {
            var emision = speedParticles.emission;
            var main = speedParticles.main;
            emision.rateOverTime = FunctionAffectedBySpeed(maxParticleEmision, minParticleEmision,actualSpeed,defaultSpeed,turboMaxSpeed);

            main.startSpeed = FunctionAffectedBySpeed(maxParticleSpeed, minParticleSpeed, actualSpeed, defaultSpeed, turboMaxSpeed);
        }
    }

    float FunctionAffectedBySpeed(float maxResult, float minResult, float currentSpeed, float minSpeed, float maxspeed)
    {
        return ((maxResult - minResult) *(currentSpeed - minSpeed) / (maxspeed - minSpeed) + minResult);
    }
    void UpdateRotation()
    {
        rotationLimit = (movement > 0) ? turboRotationLimitation : 1;

        actualRollSpeed = (roll == 0) ?
            ((actualRollSpeed < stopThreshold && actualRollSpeed > -stopThreshold) ?
                0f
                : ((actualRollSpeed > 0) ?
                    actualRollSpeed - (rollDeceleration *rotationLimit* Time.deltaTime)
                    : actualRollSpeed + (rollDeceleration * Time.deltaTime)))
            : ((actualRollSpeed < maxRollSpeed * rotationLimit && actualRollSpeed > -maxRollSpeed * rotationLimit) ?
                actualRollSpeed + (rollSpeed * rotationLimit * roll * Time.deltaTime)
                : actualRollSpeed > 0 ? maxRollSpeed * rotationLimit : -maxRollSpeed * rotationLimit);


        actualYawSpeed = (yawPicth.x == 0) ?
            ((actualYawSpeed < stopThreshold && actualYawSpeed > -stopThreshold) ?
                0f
                : ((actualYawSpeed > 0) ?
                    actualYawSpeed - (yawDeceleration * Time.deltaTime)
                    : actualYawSpeed + (yawDeceleration * Time.deltaTime)))
            : ((actualYawSpeed < maxYawSpeed * rotationLimit && actualYawSpeed > -maxYawSpeed * rotationLimit) ?
                actualYawSpeed + (yawSpeed * rotationLimit* yawPicth.x * Time.deltaTime)
                : actualYawSpeed > 0 ? maxYawSpeed * rotationLimit : -maxYawSpeed * rotationLimit);

       


        actualPitchSpeed = (yawPicth.y == 0) ?
            ((actualPitchSpeed < stopThreshold && actualPitchSpeed > -stopThreshold) ?
                0f
                : ((actualPitchSpeed > 0) ?
                    actualPitchSpeed - (pitchDeceleration * Time.deltaTime)
                    : actualPitchSpeed + (pitchDeceleration * Time.deltaTime)))
            : ((actualPitchSpeed < maxPitchSpeed * rotationLimit && actualPitchSpeed > -maxPitchSpeed * rotationLimit) ?
                actualPitchSpeed + (pitchSpeed * rotationLimit* yawPicth.y * Time.deltaTime)
                : actualPitchSpeed > 0 ? maxPitchSpeed * rotationLimit : -maxPitchSpeed * rotationLimit);



        float horizontal = -FunctionAffectedBySpeed(maxAimHorizontalMovement * rotationLimit, -maxAimHorizontalMovement * rotationLimit, actualYawSpeed, -maxYawSpeed * rotationLimit, maxYawSpeed * rotationLimit);
        float vertical = FunctionAffectedBySpeed(maxAimVerticalMovement * rotationLimit, -maxAimVerticalMovement * rotationLimit, actualPitchSpeed, -maxPitchSpeed * rotationLimit, maxPitchSpeed * rotationLimit);
        playerMoveCameraCentre.MovePosition(new Vector3(horizontal, vertical, 0));


        transform.rotation = Quaternion.AngleAxis(actualPitchSpeed, transform.right)
            * Quaternion.AngleAxis(actualYawSpeed, transform.up) 
            * Quaternion.AngleAxis(actualRollSpeed, transform.forward) 
            * transform.rotation;

    }


    private void CabinaBool(bool b)
    {
        camaraChange = b;
        Cabina?.SetActive(b);
        if (!b)
        {
            ShipPS4?.SetActive(true);
        }
        else
        {
            ShipPS4?.SetActive(false);
        }
    }

    public void chargeTurbo(TurboFuelTank fuelTank)
    {
        float turboToAdd = maxTurbo * fuelTank.turboRecoveryPercentage / 100f;

        actualTurbo += turboToAdd;
        if (actualTurbo >= maxTurbo)
            actualTurbo = maxTurbo;

        turboBar.fillAmount = actualTurbo / maxTurbo;
        fuelTank.DestroyItem();

    }

    //Imputs
    public void OnMovemente(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<float>();
        if (movement > -0.2 && movement < 0.2)
        {
            movement = 0f;
        }
    }
    public void OnRoll(InputAction.CallbackContext context)
    {
        roll = context.ReadValue<float>();
        
        if(roll>-0.2 && roll<0.2)
        {
            roll = 0f;
        }
    }
    public void OnYawOrPitch(InputAction.CallbackContext context)
    {
        yawPicth = context.ReadValue<Vector2>();

        yawPicth.x = Mathf.Clamp(yawPicth.x,-1,1);
        yawPicth.y = Mathf.Clamp(yawPicth.y, -1, 1);

        if (yawPicth.x > -0.2 && yawPicth.x < 0.2)
        {
            yawPicth.x = 0f;
        }
        if (yawPicth.y > -0.2 && yawPicth.y < 0.2)
        {
            yawPicth.y = 0f;
        }

    }
    public void OnCameraChange(InputAction.CallbackContext context)
    {
        if (camaraChange == false)
        {
            CabinaBool(true);
        }
        else if (camaraChange == true)
        {
            CabinaBool(false);
        }
    }

    


}
