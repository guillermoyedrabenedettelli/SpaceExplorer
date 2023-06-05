using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VibrationSense : byte
{
    FastPulse, SlowPulse,
    AscendingBurst,IrregularPattern,
    ExpandingWave, Spiral,
    Explosion, Throbbing, none
}

public class VibrationManage : MonoBehaviour
{
    
    [SerializeField] VibrationSense vibrationSense;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<VibrationController>())
        {
            other.GetComponent<VibrationController>().VibrationSense(vibrationSense);


        }
    }
    private void OnTriggerExit(Collider other)
    {
        /*
        if (other.GetComponent<VibrationController>())
        {
        }*/    }
}
