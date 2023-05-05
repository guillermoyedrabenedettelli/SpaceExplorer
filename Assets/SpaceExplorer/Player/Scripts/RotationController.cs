using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementController))]
public class RotationController : MonoBehaviour
{
    [SerializeField] [Range(1f, 180f)] float sensitivityX = 60f;
    [SerializeField] [Range(1f, 180f)] float sensitivityY = 60f;

    [SerializeField] [Range(1f, 3f)] float sensitivityReductionOnTurbo = 1.5f;
    [SerializeField] GameObject Ship;
    Vector2 rotation;
    [SerializeField] float anguloDeGiro;
    float anguloOriginal;
    MovementController movementController;

    float actualSensitivityReduction = 1f;


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rotation = Vector2.zero;

        movementController = GetComponent<MovementController>();

    }
    private void Start()
    {
        anguloOriginal = Ship.transform.rotation.x;
    }
    void Update()
    {
        UpdateRotation();
        //float giroEnX = 0;
        //giroEnX = rotation.x * anguloDeGiro+ giroEnX;
        //Ship.transform.rotation = Quaternion.Euler(0, 0, giroEnX);
    }

    void UpdateRotation()
    {
        float SpeedY = rotation.y * Time.deltaTime;
        float angleToApplyY = SpeedY * (sensitivityY / actualSensitivityReduction);
        float SpeedX = rotation.x * Time.deltaTime;
        float angleToApplyX = SpeedX * (sensitivityX / actualSensitivityReduction);

        actualSensitivityReduction = movementController.GetTurboOn() ? sensitivityReductionOnTurbo : 1f;

        Quaternion rotationToApplyY = Quaternion.AngleAxis(angleToApplyY, transform.right);
        Quaternion rotationToApplyX = Quaternion.AngleAxis(angleToApplyX, transform.up);

        transform.rotation = rotationToApplyY * transform.rotation;
        transform.rotation = rotationToApplyX * transform.rotation;


    }
    public void GetRotation(InputAction.CallbackContext context)
    {
        rotation = context.ReadValue<Vector2>();
        rotation.Normalize();
    }
}
