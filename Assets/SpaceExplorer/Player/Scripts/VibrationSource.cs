using UnityEngine;

public class VibrationSource : MonoBehaviour
{
    public VibrationSense vibrationSense;

    private void OnTriggerEnter(Component other)
    {
        var brr = other.GetComponent<VibrationController>();

        if (brr)
            brr.AddVibrationSource(this);
    }

    private void OnTriggerExit(Component other)
    {
        var brr = other.GetComponent<VibrationController>();

        if (brr)
            brr.RemoveVibrationSource(this);
    }
}