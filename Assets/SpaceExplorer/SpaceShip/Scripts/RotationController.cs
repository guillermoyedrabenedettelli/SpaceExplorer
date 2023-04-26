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
    Vector2 rotation;


    MovementController movementController;

    float actualSensitivityReduction = 1f;


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rotation = Vector2.zero;

        movementController = GetComponent<MovementController>();

    }
    void Update()
    {
        UpdateRotation();
    }

    void UpdateRotation()
    {
        if (movementController.GetTurboOn())
        {
            actualSensitivityReduction = sensitivityReductionOnTurbo;
        }
        else
        {
            actualSensitivityReduction = 1f;
        }
        if (rotation.y != 0f)
        {
            float SpeedY = (rotation.y / Screen.height) / Time.deltaTime;
            float angleToApplyY = SpeedY * (sensitivityY / actualSensitivityReduction);
            Quaternion rotationToApplyY = Quaternion.AngleAxis(angleToApplyY, transform.right);
            transform.rotation = rotationToApplyY * transform.rotation;
        }
        if (rotation.x != 0f)
        {
            float SpeedX = (rotation.x / Screen.width) / Time.deltaTime;
            float angleToApplyX = SpeedX * (sensitivityX / actualSensitivityReduction);
            Quaternion rotationToApplyX = Quaternion.AngleAxis(angleToApplyX, transform.up);
            transform.rotation = rotationToApplyX * transform.rotation;
        }
    }
    public void GetRotation(InputAction.CallbackContext context)
    {
        rotation=context.ReadValue<Vector2>();
        rotation.Normalize();    
    }
}
