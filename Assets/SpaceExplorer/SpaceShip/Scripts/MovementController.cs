using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{

    [SerializeField] Transform characterTransform;

    [Header("Spaceship Acceleration")]
    [SerializeField] float acceleration = 0.05f;
    [SerializeField] float turboAcceleration = 1f;
    [SerializeField] float maxSpeedWithoutTurbo = 10f;
    [SerializeField] float maxSpeedWithTurbo = 20f;
    //acceleration reduction when inputs stop
    [SerializeField] float deceleration = 0.05f;


    CharacterController characterController;

    [SerializeField] Vector3 actualAcceleration;

    Vector3 motion;


    [Header("Turbo configuration")]
    [SerializeField] float maxTurbo = 100f;
    [SerializeField] float turboConsumption =6f;
    [SerializeField] float turboRecovery = 2f;
    [SerializeField] float actualTurbo;
    [SerializeField] float timeToStartRecovering =2f;
    float actualTimeWaitingForRecover = 0f;
    [SerializeField] Image turboBar;


    [Header("Direction Bindings")]
    [SerializeField] InputAction forwardInput;
    [SerializeField] InputAction backwardInput;
    [SerializeField] InputAction upInput;
    [SerializeField] InputAction downInput;
    [SerializeField] InputAction rightInput;
    [SerializeField] InputAction leftInput;

    [Header("Turbo Bindings")]
    [SerializeField] InputAction turboInput;


    private void Awake()
    {
        //Initialize
        characterController = GetComponent<CharacterController>();
        actualAcceleration = Vector3.zero;

        EnableInputs();

        actualTurbo = maxTurbo;
    }

    private void Update()
    {
        UpdateMovement();
    }

    bool TurboOn = false;
    private void UpdateMovement()
    {
        motion = Vector3.zero;

        TurboUpdate();
        getInputAccelerations();

        motion += characterTransform.right * actualAcceleration.x * Time.deltaTime;
        motion += characterTransform.up * actualAcceleration.y * Time.deltaTime;
        motion += -characterTransform.forward * actualAcceleration.z * Time.deltaTime;


        characterController.Move(motion);
    }

    void EnableInputs()
    {
        forwardInput.Enable();
        backwardInput.Enable();
        upInput.Enable();
        downInput.Enable();
        rightInput.Enable();
        leftInput.Enable();

        turboInput.Enable();
    }

    void getInputAccelerations()
    {
        if (leftInput.IsPressed())
        {
            if (actualAcceleration.x < maxSpeedWithoutTurbo)
            {
                actualAcceleration.x += acceleration;
            }
        }
        else if (rightInput.IsPressed())
        {
            if (actualAcceleration.x > -maxSpeedWithoutTurbo)
            {
                actualAcceleration.x += -acceleration;
            }
        }
        else
        {
            if (actualAcceleration.x < (-0.4f))
            {
                actualAcceleration.x += (deceleration);
            }
            else if (actualAcceleration.x > (0.4f))
            {
                actualAcceleration.x += (-deceleration);
            }
            else
            {
                actualAcceleration.x = 0;
            }
        }
        if (upInput.IsPressed())
        {
            if (actualAcceleration.y < maxSpeedWithoutTurbo)
            {
                actualAcceleration.y += acceleration;
            }
        }
        else if (downInput.IsPressed())
        {
            if (actualAcceleration.y > -maxSpeedWithoutTurbo)
            {
                actualAcceleration.y += -acceleration;
            }
        }
        else
        {
            if (actualAcceleration.y > (0.4f))
            {
                actualAcceleration.y += (-deceleration);
            }
            else if (actualAcceleration.y < (-0.4f))
            {
                actualAcceleration.y += (deceleration);
            }
            else
            {
                actualAcceleration.y = 0;
            }
        }
        if (!TurboOn)
        {
            if (forwardInput.IsPressed())
            {
                if (actualAcceleration.z < maxSpeedWithoutTurbo)
                {
                    actualAcceleration.z += acceleration;
                }
            }
            else if (backwardInput.IsPressed())
            {
                if (actualAcceleration.z > -maxSpeedWithoutTurbo)
                {
                    actualAcceleration.z += -acceleration;
                }
            }
            else
            {
                if (actualAcceleration.z > (0.4f))
                {
                    actualAcceleration.z += (-deceleration);
                }
                else if (actualAcceleration.z < (-0.4f))
                {
                    actualAcceleration.z += (deceleration);
                }
                else
                {
                    actualAcceleration.z = 0;
                }
            }
        }
        else
        {
            if (actualAcceleration.z < maxSpeedWithTurbo)
            {
                actualAcceleration.z += turboAcceleration;
            }
        }
    }

    void TurboUpdate()
    {
        if (turboInput.IsPressed() && actualTurbo >= turboConsumption * Time.deltaTime)
        {
            actualTurbo -= turboConsumption * Time.deltaTime;
            if (turboBar != null)
            {
                turboBar.fillAmount = actualTurbo / maxTurbo;
            }
            if (actualTurbo <= 0f)
            {
                actualTurbo = 0f;
            }
            TurboOn = true;
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
                    FillTurboBar();
                }
            }
            else
            {
                actualTimeWaitingForRecover += Time.deltaTime;
            }

            TurboOn = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("TurboFuelTank"))
        {
            TurboFuelTank fuelTank=other.GetComponent<TurboFuelTank>();
            float turboToAdd=maxTurbo*fuelTank.turboRecoveryPercentage/100f;

            actualTurbo += turboToAdd;
            if (actualTurbo >= maxTurbo)
                actualTurbo = maxTurbo;

            FillTurboBar();

            fuelTank.DestroyItem();
        }
    }

    void FillTurboBar()
    {
        if (turboBar != null)
        {
            turboBar.fillAmount = actualTurbo / maxTurbo;
        }
    }


    public bool GetTurboOn()
    {
        return TurboOn;
    }
}
