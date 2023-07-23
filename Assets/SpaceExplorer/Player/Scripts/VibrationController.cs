using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class VibrationController : MonoBehaviour
{
    [SerializeField] private float vibrationDuration = 1f;
    [SerializeField] private float vibrationIntensity = 0.5f;
    [SerializeField] private AnimationCurve vibrationCurve = new();

    public float startTime = 0f;
    public float endTime = 1f;
    public int numSamples = 100000;

    private readonly HashSet<VibrationSource> sources = new();
    private bool isVibrating = false;

    private void Awake()
    {
        SetVibrationCurve();

        InputSystem.onDeviceChange += OnDeviceChanged;
    }

    /// <summary> Handles device changes, such as disconnecting the main gamepad. </summary>
    /// <param name="device"> The device that has changed. </param>
    /// <param name="change"> The change the device has suffered. </param>
    private void OnDeviceChanged(InputDevice device, InputDeviceChange change)
    {
        // If main gamepad got disconnected, stop vibrating
        if (Gamepad.current == null)
        {
            StopVibration();
            return;
        }

        UpdateVibration();
    }

    /// <summary> Vibration sources add themselves through their colliders. </summary>
    /// <param name="source"> Vibration source to add. </param>
    public void AddVibrationSource(VibrationSource source)
    {
        Debug.Log("[VibrationController] Add Vibration Source");
        sources.Add(source);
        UpdateVibration();
    }

    /// <summary> Vibration sources remove themselves through their colliders. </summary>
    /// <param name="source"> Vibration source to remove. </param>
    public void RemoveVibrationSource(VibrationSource source)
    {
        Debug.Log("[VibrationController] Remove Vibration Source");
        sources.Remove(source);
        UpdateVibration();
    }

    /// <summary> Checks if there are any vibration sources and updates vibration accordingly. </summary>
    private void UpdateVibration()
    {
        Debug.Log("[VibrationController] Update Vibration, Source count:" + sources.Count);
        if (sources.Count > 0)
            SetVibration(sources.Last().vibrationSense);
        else
            StopVibration();
    }

    private void SetVibrationCurve()
    {
        for (var i = 0; i <= numSamples; i++)
        {
            var time = Mathf.Lerp(startTime, endTime, (float)i / numSamples);
            var value = Sigmoid(time);

            var keyframe = new Keyframe(time, value);
            vibrationCurve.AddKey(keyframe);
        }
    }

    private static float Sigmoid(float x)
    {
        return 1 / (1 + Mathf.Exp(-x));
    }

    private void OnDisable()
    {
        sources.Clear();
        StopVibration();
    }

    private void SetVibration(VibrationSense sense)
    {
        if (isVibrating) return;
        
        isVibrating = true;

        switch (sense)
        {
            case VibrationSense.FastPulse:
                VibrateFastPulse();
                break;
            case VibrationSense.SlowPulse:
                VibrateSlowPulse();
                break;
            case VibrationSense.AscendingBurst:
                VibrateAscendingBurst();
                break;
            case VibrationSense.IrregularPattern:
                VibrateIrregularPattern();
                break;
            case VibrationSense.ExpandingWave:
                VibrateExpandingWave();
                break;
            case VibrationSense.Spiral:
                VibrateSpiral();
                break;
            case VibrationSense.Explosion:
                VibrateExplosion();
                break;
            case VibrationSense.Throbbing:
                VibrateThrobbing();
                break;
            case VibrationSense.Default:
            case VibrationSense.None:
                Debug.LogWarning("[VibrationController] VibrationSense is set to None or Default.");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void StopVibration()
    {
        isVibrating = false;

        if (Gamepad.current != null)
            Gamepad.current.ResetHaptics();

        Debug.Log("[VibrationController] Stop Vibration");
    }

    private async void VibrateFastPulse()
    {
        while (isVibrating)
        {
            var s = Stopwatch.StartNew();
            
            if (Gamepad.current != null)
                Gamepad.current.SetMotorSpeeds(0.8f, 0.8f);

            await Task.Delay(100);
            
            Debug.Log("Awaited for: " + s.ElapsedMilliseconds + "ms");

            if (Gamepad.current != null)
                Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);

            await Task.Delay(200);

            Debug.Log("Awaited for: " + s.ElapsedMilliseconds + "ms");
        }

        if (Gamepad.current != null)
            Gamepad.current.ResetHaptics();
    }

    private async void VibrateSlowPulse()
    {
        while (isVibrating)
        {
            if (Gamepad.current != null)
                Gamepad.current.SetMotorSpeeds(0.3f, 0.3f);

            await Task.Delay(500);

            if (Gamepad.current != null)
                Gamepad.current.ResetHaptics();

            await Task.Delay(1000);
        }

        if (Gamepad.current != null)
            Gamepad.current.ResetHaptics();
    }

    private async void VibrateAscendingBurst()
    {
        startTime = Time.time;

        while (isVibrating)
        {
            var elapsedTime = Time.time - startTime;
            var normalizedTime = Mathf.Clamp01(elapsedTime / vibrationDuration);
            var vibrationValue = vibrationCurve.Evaluate(normalizedTime) * vibrationIntensity;

            if (Gamepad.current != null)
                Gamepad.current.SetMotorSpeeds(vibrationValue, vibrationValue);

            await Task.Yield();
        }

        if (Gamepad.current != null)
            Gamepad.current.ResetHaptics();
    }

    private async void VibrateIrregularPattern()
    {
        while (isVibrating)
        {
            var randomDelay = Random.Range(100, 500);
            Gamepad.current.SetMotorSpeeds(0.6f, 0.6f);
            await Task.Delay(randomDelay);
            Gamepad.current.ResetHaptics();
            await Task.Delay(randomDelay);
        }

        if (Gamepad.current != null)
            Gamepad.current.ResetHaptics();
    }

    private async void VibrateExpandingWave()
    {
        var vec = new Vector2();
        startTime = Time.time;

        while (isVibrating)
        {
            var elapsedTime = Time.time - startTime;
            var normalizedTime = Mathf.Clamp01(elapsedTime / vibrationDuration);
            var vibrationValue = vibrationCurve.Evaluate(normalizedTime) * vibrationIntensity;

            vec.x = Mathf.Sin(elapsedTime);
            vec.y = Mathf.Cos(elapsedTime);

            Gamepad.current.SetMotorSpeeds(vibrationValue * vec.x, vibrationValue * vec.y);
            await Task.Yield();
        }

        if (Gamepad.current != null)
            Gamepad.current.ResetHaptics();
    }

    private async void VibrateSpiral()
    {
        startTime = Time.time;

        while (isVibrating)
        {
            float elapsedTime = Time.time - startTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / vibrationDuration);
            float vibrationValue = vibrationCurve.Evaluate(normalizedTime) * vibrationIntensity;

            Vector2 normalizedPosition =
                new Vector2(Mathf.Sin(elapsedTime * 3f), Mathf.Cos(elapsedTime * 3f)).normalized;
            Gamepad.current.SetMotorSpeeds(vibrationValue * normalizedPosition.x,
                vibrationValue * normalizedPosition.y);
            await Task.Yield();
        }

        if (Gamepad.current != null)
            Gamepad.current.ResetHaptics();
    }

    private async void VibrateExplosion()
    {
        Gamepad.current.SetMotorSpeeds(1f, 1f);

        await Task.Delay(200);

        if (Gamepad.current != null)
            Gamepad.current.ResetHaptics();
    }

    private async void VibrateThrobbing()
    {
        startTime = Time.time;

        var maxVibrationIntensity = vibrationIntensity * 0.8f;
        var pulseDuration = vibrationDuration / 2f;

        while (isVibrating)
        {
            var elapsedTime = Time.time - startTime;
            var normalizedTime = Mathf.Clamp01(elapsedTime / pulseDuration);

            var vibrationValue = Mathf.Lerp(0f, maxVibrationIntensity, normalizedTime);
            Gamepad.current.SetMotorSpeeds(vibrationValue, vibrationValue);

            if (normalizedTime >= 1f)
            {
                var decreaseTime = elapsedTime - pulseDuration;
                var normalizedDecreaseTime = Mathf.Clamp01(decreaseTime / pulseDuration);

                var decreaseVibrationValue = Mathf.Lerp(maxVibrationIntensity, 0f, normalizedDecreaseTime);
                Gamepad.current.SetMotorSpeeds(decreaseVibrationValue, decreaseVibrationValue);

                if (normalizedDecreaseTime >= 1f)
                    startTime = Time.time;
            }

            await Task.Yield();
        }

        if (Gamepad.current != null)
            Gamepad.current.ResetHaptics();
    }
}