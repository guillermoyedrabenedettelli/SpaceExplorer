using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DualSenseSample.Inputs.DualSenseTouchpadColor))]
[RequireComponent(typeof(DualSenseSample.Inputs.DualSenseRumble))]
[RequireComponent(typeof(DualSenseSample.Inputs.DualSenseTrigger))]
[RequireComponent(typeof(DualSenseSample.Inputs.DualSenseMonitor))]
public class SpaceDualSense : MonoBehaviour
{
    // Referencia al script DualSenseTouchpadColor

    [SerializeField] DualSenseSample.Inputs.DualSenseTouchpadColor touchpadColorScript;
    [SerializeField] DualSenseSample.Inputs.DualSenseRumble dualSenseRumble;
    [SerializeField] DualSenseSample.Inputs.DualSenseTrigger dualSenseTrigger;

    public float redValue = 0.5f;
    public float greenValue = 0.5f;
    public float blueValue = 0.5f;

    public float leftRumble = 0f;        
    public float rightRumble = 0f;


    public float leftTriggeretForce = 0f;
    public float rightTriggeretForce = 100f;         

    // (solo de 0 a 2 para activar algun efecto)
    public int leftTriggerEffectType = 0;
    public int rightTriggerEffectType = 2;

    //(si hay effecto tipo 2 -> EffectEx) ajustar parametros

    public float frequencyRigth = 0.05f;
    public float frequencyLeft = 0f;

    public float RightEffectStartPosition = 0f;
    public float RightEffectBeginForce = 0f;
    public float RightEffectMiddleForce = 50;
    public float RightEffectEndForce = 1;

   
    private void Awake()
    {
        //ShootMotionOff();
    }
    private void Start()
    {
        // Encuentra la instancia de DualSenseTouchpadColor en la escena y guárdala en la variable touchpadColorScript
        dualSenseRumble = FindObjectOfType<DualSenseSample.Inputs.DualSenseRumble>();
    }
    private void Update()
    {
        // Ejemplo: Modificar el color rojo al valor 0.5f

        touchpadColorScript.UpdateRedColor(redValue);
        touchpadColorScript.UpdateGreenColor(greenValue);
        touchpadColorScript.UpdateBlueColor(blueValue);
        dualSenseRumble.LeftRumble = leftRumble;    //(0 - 1) 0 apagado 1 -> 100% prendido
        dualSenseRumble.RightRumble = rightRumble;  //(0 - 1) 0 apagado 1 -> 100% prendido
        /*
         * Ejemplo de como acceder a los parametros de efectos del Dualsense
         */
       
                 dualSenseTrigger.LeftContinuousForce = leftTriggeretForce;
                 dualSenseTrigger.RightContinuousForce = rightTriggeretForce;

                 dualSenseTrigger.LeftTriggerEffectType = leftTriggerEffectType;
                 dualSenseTrigger.RightTriggerEffectType = rightTriggerEffectType;

                 dualSenseTrigger.RightEffectFrequency = frequencyRigth;
                 dualSenseTrigger.LeftEffectFrequency = frequencyLeft;

                 dualSenseTrigger.RightEffectStartPosition = RightEffectStartPosition;
                 dualSenseTrigger.RightEffectBeginForce = RightEffectBeginForce;
                 dualSenseTrigger.RightEffectMiddleForce = RightEffectMiddleForce;
                 dualSenseTrigger.RightEffectEndForce = RightEffectEndForce;
      
       // ShootMotion();
    }
    private void ShootMotion()
    {
        dualSenseTrigger.LeftContinuousForce = leftTriggeretForce;
        dualSenseTrigger.RightContinuousForce = rightTriggeretForce;

        dualSenseTrigger.LeftTriggerEffectType = leftTriggerEffectType;
        dualSenseTrigger.RightTriggerEffectType = rightTriggerEffectType;

        dualSenseTrigger.RightEffectFrequency = frequencyRigth;
        dualSenseTrigger.LeftEffectFrequency = frequencyLeft;

        dualSenseTrigger.RightEffectStartPosition = RightEffectStartPosition;
        dualSenseTrigger.RightEffectBeginForce = RightEffectBeginForce;
        dualSenseTrigger.RightEffectMiddleForce = RightEffectMiddleForce;
        dualSenseTrigger.RightEffectEndForce = RightEffectEndForce;
    }
    void ShootMotionOff()
    {
        dualSenseTrigger.RightContinuousForce = 0.0f;
        dualSenseTrigger.RightTriggerEffectType = 3;
        dualSenseTrigger.RightEffectFrequency = 0.0f;
        dualSenseTrigger.RightEffectStartPosition = 0.0f;
        dualSenseTrigger.RightEffectBeginForce = 0.0f;
        dualSenseTrigger.RightEffectMiddleForce = 0.0f;
        dualSenseTrigger.RightEffectEndForce = 0.0f;
    }
}
