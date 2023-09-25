using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;
using Image = UnityEngine.UI.Image;

public enum LandingStepsEnum:int
{
    Rotate, Position, LookAt, Move, Land, Landed, TakeOff, Count

}


public class PlayerMovementController : MonoBehaviour
{
    [Header("Pause Menu")]
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] PauseMenu deathMenu;

    [Header("Rotation")]
    [SerializeField] float turboRotationLimitation = 0.5f;
    private float rotationLimit = 1f;
    [SerializeField] float stopThreshold = 0.2f;
    [SerializeField] float rotationDeadZone = 0.5f;

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
    [SerializeField] float defaultSpeed = 2f;
    [SerializeField] float defaultAcceleration = 0.2f;
    [SerializeField] float turboMaxSpeed = 40f;
    [SerializeField] float actualSpeed = 2f;
    [SerializeField] float turboAceleration = 0.5f;
    [SerializeField] float turboDeceleration = 0.2f;
    [SerializeField] float maxTurbo = 100f;
    [SerializeField] float actualTurbo = 0;
    [SerializeField] float turboReductionPerSecond = 3f;
    [SerializeField] Image turboBar;
    [SerializeField] float turboRecovery = 2f;
    [SerializeField] float timeToStartRecovering = 2f;
    float actualTimeWaitingForRecover = 0f;
    [SerializeField] float movementDeadzone = 0.2f;
    Quaternion shipStartRotation;
    [SerializeField] float rotationOnYaw = 10f;


    [Header("Particles")]
    [SerializeField] float maxParticleSpeed = 25;
    [SerializeField] float minParticleSpeed = 10;
    [SerializeField] float maxParticleEmision = 20f;
    [SerializeField] float minParticleEmision = 10f;
    [SerializeField] ParticleSystem speedParticles;


    [Header("Camera")]
    [SerializeField] GameObject Cabina;
    [SerializeField] GameObject ShipPS4;
    bool camaraChange = false;
    [SerializeField] float maxAimHorizontalMovement = 2f;
    [SerializeField] float maxAimVerticalMovement = 1f;

    [Header("Tow Mission Variables")]
    float towCurrentTime = 0f;
    bool canTow = false;
    bool towing = false;

    [SerializeField] TowArea towArea;
    [SerializeField] float towTime = 5f;
    [SerializeField] GameObject towPoint;


    [Header("Landing")]
    [SerializeField] GameObject landingMessage = null;
    [SerializeField] float heightBeforeLanding = 2f;
    [SerializeField] LandingStepsEnum landingState = LandingStepsEnum.Rotate;
    Vector3 startTakeOffPosition;
    float time=0f;
    float aligmentSpeed=1f;
    Quaternion rollPitchRotation;
    Quaternion initialRotatiom;
    Quaternion lookAt;
    bool isReadyToLand = false;
    public bool isLanding = false;
    GameObject landCamera;
    TrailRenderer[] trails;

    Transform landingTarget;

    Rigidbody rigidBody;
    CharacterController ch;
    PlayerMoveCameraCentre playerMoveCameraCentre;

    private float movement;
    private float roll;
    private Vector2 yawPicth;
    private Misiones3 Missions;
    private WeaponsShip Weapons;

    public static GameObject currentObjetive;

    




    void Awake()
    {
        roll = 0f;
        movement = 0f;
        yawPicth = new Vector2(0, 0);

        CabinaBool(false);
        actualTurbo = maxTurbo;
        ch = GetComponent<CharacterController>();
        shipStartRotation = ShipPS4.transform.localRotation;

        playerMoveCameraCentre = GetComponentInChildren<PlayerMoveCameraCentre>();

        if(landingMessage!=null)
            landingMessage.SetActive(false);
        trails = GetComponentsInChildren<TrailRenderer>();

        Missions = this.GetComponent<Misiones3>();
        Weapons=GetComponent<WeaponsShip>();

        Checkpoint.checkpointPosition = transform.position;

        if (deathMenu != null)
        {
            deathMenu.SetRequeriments(Weapons, this);

        }
        if (pauseMenu != null)
        {
            pauseMenu.SetRequeriments(Weapons, this);
        }

    }

    void Update()
    {
        if (!isLanding)
        {
            UpdateLocation();
            UpdateSpeedParticles();
            UpdateRotation();
            if(towing)
            {
                towCurrentTime = towCurrentTime + Time.deltaTime;
                towArea.UpdateProgressBar(towCurrentTime/towTime>=1?1: towCurrentTime / towTime);
                if (towCurrentTime >= towTime)
                {
                    /*FixedJoint fj=towPoint.AddComponent<FixedJoint>();*/
                    towArea.GetTowItem().transform.position = towPoint.transform.position;
                    towArea.GetTowItem().transform.parent = towPoint.transform;
                    /*fj.connectedBody = towArea.GetTowItem().GetComponent<Rigidbody>();*/

                    //towArea.GetTowItem().transform.parent = transform;
                    towArea.SetTowed();
                    canTow = false;
                    towing = false;
                    Missions.actualizaMision(4);
                }
            }
        }
        else
        {
            switch (landingState)
            {
                case LandingStepsEnum.Rotate:
                    RotateToLand();
                    break;
                case LandingStepsEnum.Position:
                    MoveToPosition(new Vector3(transform.position.x, landingTarget.position.y + heightBeforeLanding, transform.position.z));
                    break;
                case LandingStepsEnum.LookAt:
                    LookToTarget();
                    break;
                case LandingStepsEnum.Move:
                    MoveToPosition(new Vector3(landingTarget.position.x, transform.position.y, landingTarget.position.z));
                    break;
                case LandingStepsEnum.Land:
                    Land();
                    break;
                case LandingStepsEnum.TakeOff:
                    TakeOff();
                    break;

            }
            turboFuelcharge();
        }
        
    }

    //Movement
    void UpdateLocation()
    {
        if (movement < 0)
        {
            actualSpeed = ((actualSpeed > 0.2) ? actualSpeed - (turboDeceleration) : 0);
        }
        else if (movement == 0 && actualSpeed < defaultSpeed)
        {
            actualSpeed = (actualSpeed + defaultSpeed > defaultSpeed) ? defaultSpeed : actualSpeed + defaultAcceleration;
        }
        else if(movement>0 && actualTurbo>0 )
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
            turboFuelcharge();
            actualSpeed =(actualSpeed > defaultSpeed) ? actualSpeed - turboDeceleration : defaultSpeed;
        }

        ch.Move(-transform.forward * actualSpeed * Time.deltaTime);
    }

    void UpdateSpeedParticles()
    {
        if (speedParticles != null)
        {
            var emision = speedParticles.emission;
            var main = speedParticles.main;
            emision.rateOverTime = FunctionAffectedBySpeed(maxParticleEmision, minParticleEmision,actualSpeed,0f,turboMaxSpeed);

            main.startSpeed = FunctionAffectedBySpeed(maxParticleSpeed, minParticleSpeed, actualSpeed, 0f, turboMaxSpeed);
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


        transform.rotation = Quaternion.AngleAxis(actualPitchSpeed *Time.deltaTime, transform.right)
            * Quaternion.AngleAxis(actualYawSpeed* Time.deltaTime, transform.up) 
            * Quaternion.AngleAxis(actualRollSpeed* Time.deltaTime, transform.forward) 
            * transform.rotation;

        RollOnYaw();

    }

    void RollOnYaw()
    {
        if(actualYawSpeed>=0)
        {
            ShipPS4.transform.localRotation = Quaternion.Euler(Mathf.LerpAngle(shipStartRotation.eulerAngles.x, shipStartRotation.eulerAngles.x + rotationOnYaw, (actualYawSpeed / maxYawSpeed)), ShipPS4.transform.localRotation.eulerAngles.y, ShipPS4.transform.localRotation.eulerAngles.z);
        }
        else
        {
            ShipPS4.transform.localRotation = Quaternion.Euler(Mathf.LerpAngle(shipStartRotation.eulerAngles.x, shipStartRotation.eulerAngles.x - rotationOnYaw, -1*(actualYawSpeed / maxYawSpeed)), ShipPS4.transform.localRotation.eulerAngles.y, ShipPS4.transform.localRotation.eulerAngles.z);
        }

    }

    //Camera Methods
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

    //TurboMethods
    void turboFuelcharge()
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

    public void FullChargeTurbo()
    {
        actualTurbo = maxTurbo;
        turboBar.fillAmount = 1;
    }

    //Landing methods
    public void SetLandingState(LandingStepsEnum newLandingState)
    {
        landingState=newLandingState;
    }
    public void ReadyToLand(bool ready,Transform target,GameObject landingCamera)
    {
        if (!isLanding)
        {
            isReadyToLand = ready;
            landingTarget = target;
            landCamera = landingCamera;
            if (landingMessage!=null)
                landingMessage.SetActive(ready);
        }
    }
    public void SetReadyToLand(bool ready)
    {
        isReadyToLand = ready;
        if (landingMessage != null)
            landingMessage.SetActive(ready);
    }
    void StartLanding()
    {
        isLanding = true;
        speedParticles.gameObject.SetActive(false);
        if (landCamera != null)
            landCamera.gameObject.SetActive(true);
        if (landingMessage != null)
            landingMessage.SetActive(false);
        if (Weapons != null)
            Weapons.enabled = false;
    }


    void RotateToLand()
    {
        if (time < 1)
        {
            time = (aligmentSpeed / 2 * Time.deltaTime > 1) ? 1 : time + (aligmentSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(initialRotatiom, rollPitchRotation, time);
        }
        else
        {
            landingState++;
            time = 0;
        }
    }


    bool MoveToPosition(Vector3 targetPosition)
    {
        Vector3 offset = targetPosition - transform.position;

        if (offset.magnitude > .1f)
        {
            offset = offset.normalized * defaultSpeed / 2;
            ch.Move(offset * Time.deltaTime);
            return false;
        }
        else
        {
            lookAt = Quaternion.LookRotation(transform.position - new Vector3(landingTarget.position.x, transform.position.y, landingTarget.position.z));
            time = 0;
            initialRotatiom = transform.rotation;
            landingState++;
            return true;
        }
    }
    void LookToTarget()
    {
        if (time < 1)
        {
            time = (aligmentSpeed / 2 * Time.deltaTime > 1) ? 1 : time + (aligmentSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(initialRotatiom, lookAt, time);
        }
        else
        {
            //transform.parent = landingTarget;
            landingState++;
            time = 0;
        }

    }

    void Land()
    {
        var offset = landingTarget.position - transform.position;

        if (Vector3.Distance(transform.position, landingTarget.position)>0.5)
        {
            transform.position = Vector3.MoveTowards(transform.position, landingTarget.position, defaultSpeed/2 * Time.deltaTime);
        }
        else
        {
            landingState++;
            SaveCheckpoint();
            
            if (Missions.ActiveNextConversation())
            {
                this.enabled = false;
            }
            
            

            foreach (TrailRenderer trail in trails)
            {
                trail.gameObject.SetActive(false);
            }
        }
    }

    void TakeOff()
    {
        if (startTakeOffPosition != null)
        {
            if (MoveToPosition(new Vector3(startTakeOffPosition.x, startTakeOffPosition.y + heightBeforeLanding, startTakeOffPosition.z)))
            {
                foreach (TrailRenderer trail in trails)
                {
                    trail.gameObject.SetActive(true);
                }
                speedParticles.gameObject.SetActive(true);
                landCamera.SetActive(false);
                isLanding = false;
            }
        }
        else
        {
            foreach (TrailRenderer trail in trails)
            {
                trail.gameObject.SetActive(true);
            }
            speedParticles.gameObject.SetActive(true);
            landCamera.SetActive(false);
            isLanding = false;
        }
           
        
    }

    //Checkpoint
    void SaveCheckpoint()
    {
        Checkpoint.checkpointPosition = transform.position;
        Checkpoint.newchekpoint = true;
    }



    //Inputs
    public void OnMovemente(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<float>();
        if (movement > -movementDeadzone && movement < movementDeadzone)
        {
            movement = 0f;
        }
    }
    public void OnRoll(InputAction.CallbackContext context)
    {
        roll = context.ReadValue<float>();
        
        if(roll>-rotationDeadZone && roll< rotationDeadZone)
        {
            roll = 0f;
        }
    }
    public void OnYawOrPitch(InputAction.CallbackContext context)
    {
        yawPicth = context.ReadValue<Vector2>();

        yawPicth.x = Mathf.Clamp(yawPicth.x,-1,1);
        yawPicth.y = Mathf.Clamp(yawPicth.y, -1, 1)*-1;

        if (yawPicth.x > -rotationDeadZone && yawPicth.x < rotationDeadZone)
        {
            yawPicth.x = 0f;
        }
        if (yawPicth.y > -rotationDeadZone && yawPicth.y < rotationDeadZone)
        {
            yawPicth.y = 0f;
        }

    }
    public void OnCameraChange(InputAction.CallbackContext context)
    {
        if (!isLanding)
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

    public void OnPressActionButton(InputAction.CallbackContext context)
    {
        if (canTow)
        {
            if (context.started)
            {
                towing = true;
            }
            if(context.canceled)
            {
                towing = false;
            }
            towCurrentTime = 0f;
            towArea.UpdateProgressBar(0);

        }
        else
        {
            if (context.canceled && isReadyToLand && !isLanding)
            {
                StartLanding();
                rollPitchRotation = Quaternion.Euler(landingTarget.rotation.x, transform.rotation.y, landingTarget.rotation.z);
                initialRotatiom = transform.rotation;
                landingState = LandingStepsEnum.Rotate;
          
                //Only do something if the mission is the inserted
                UpdateCurrentMission(1);
                UpdateCurrentMission(3);
                UpdateCurrentMission(5);
                

                Weapons.enabled = false;

            }
            else if (landingState == LandingStepsEnum.Landed)
            {
                landingState++;
                startTakeOffPosition = transform.position;
                Weapons.enabled = true;
            }
        }
        
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.canceled && !isLanding)
        {
            pauseMenu.PauseGame();
        }
    }

    //Will be called from ConversationManager
    public void StartTakeOf()
    {
       if (landingState == LandingStepsEnum.Landed)
        {
            landingState++;
            startTakeOffPosition = transform.position;
            Weapons.enabled = true;
        }
    }

    public void UpdateCurrentMission(int missioniId)
    {
        if (Missions.GetCurrentMission() == missioniId)
        {
            Missions.actualizaMision(Missions.GetCurrentMission());
        }
    }


    //Tow Methods

    public void SetCanTow(bool newCanTow)
    {
        canTow = newCanTow;
        towing = false;
        towCurrentTime = 0f;
        towArea.UpdateProgressBar(0f);

    }

    public void SetTowItem(TowArea newTowArea)
    {
        towArea = newTowArea;
    }


}
