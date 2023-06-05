using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationController : MonoBehaviour
{
    public Transform targetZone;
    public float vibrationDuration = 1f;
    public float vibrationIntensity = 0.5f;
    public AnimationCurve vibrationCurve;

    private Gamepad gamepad;
    private bool isVibrating = false;
    private float vibrationTimer = 0f;

    private void Start()
    {
        // Obtén el gamepad principal
        gamepad = Gamepad.current;
    }

    private void Update()
    {
        // Verifica si el gamepad existe y si el jugador se encuentra cerca de la zona objetivo
        if (gamepad != null && Vector3.Distance(transform.position, targetZone.position) < 2f)
        {
            if (!isVibrating)
            {
                // Comienza una subrutina de vibración aleatoria
                isVibrating = true;
                vibrationTimer = 0f;

                int vibrationIndex = Random.Range(0, 8);
                switch (vibrationIndex)
                {
                    case 0:
                        StartCoroutine(VibrateFastPulse());
                        Debug.Log("Reproduciendo: Ráfaga rápida");
                        break;
                    case 1:
                        StartCoroutine(VibrateSlowPulse());
                        Debug.Log("Reproduciendo: Pulso tranquilo");
                        break;
                    case 2:
                        StartCoroutine(VibrateAscendingBurst());
                        Debug.Log("Reproduciendo: Ráfaga ascendente");
                        break;
                    case 3:
                        StartCoroutine(VibrateIrregularPattern());
                        Debug.Log("Reproduciendo: Vibración irregular");
                        break;
                    case 4:
                        StartCoroutine(VibrateExpandingWave());
                        Debug.Log("Reproduciendo: Onda expansiva");
                        break;
                    case 5:
                        StartCoroutine(VibrateSpiral());
                        Debug.Log("Reproduciendo: Vibración en espiral");
                        break;
                    case 6:
                        StartCoroutine(VibrateExplosion());
                        Debug.Log("Reproduciendo: Vibración de explosión");
                        break;
                    case 7:
                        StartCoroutine(VibrateThrobbing());
                        Debug.Log("Reproduciendo: Vibración de Throbbing");
                        break;

                }
            }
        }
        else if (isVibrating)
        {
            // Detiene la vibración
            isVibrating = false;
            gamepad.ResetHaptics();
            Debug.Log("Deteniendo vibración");
        }
    }

    private IEnumerator VibrateFastPulse()
    {
        while (isVibrating)
        {
            gamepad.SetMotorSpeeds(0.8f, 0.8f);
            yield return new WaitForSeconds(0.1f);
            gamepad.ResetHaptics();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator VibrateSlowPulse()
    {
        while (isVibrating)
        {
            gamepad.SetMotorSpeeds(0.3f, 0.3f);
            yield return new WaitForSeconds(0.5f);
            gamepad.ResetHaptics();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator VibrateAscendingBurst()
    {
        float startTime = Time.time;
        while (isVibrating)
        {
            float elapsedTime = Time.time - startTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / vibrationDuration);
            float vibrationValue = vibrationCurve.Evaluate(normalizedTime) * vibrationIntensity;

            gamepad.SetMotorSpeeds(vibrationValue, vibrationValue);
            yield return null;
        }
        gamepad.ResetHaptics();
    }

    private IEnumerator VibrateIrregularPattern()
    {
        while (isVibrating)
        {
            float randomDelay = Random.Range(0.1f, 0.5f);
            gamepad.SetMotorSpeeds(0.6f, 0.6f);
            yield return new WaitForSeconds(randomDelay);
            gamepad.ResetHaptics();
            yield return new WaitForSeconds(randomDelay);
        }
    }

    private IEnumerator VibrateExpandingWave()
    {
        float startTime = Time.time;
        while (isVibrating)
        {
            float elapsedTime = Time.time - startTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / vibrationDuration);
            float vibrationValue = vibrationCurve.Evaluate(normalizedTime) * vibrationIntensity;

            Vector2 normalizedPosition = new Vector2(Mathf.Sin(elapsedTime), Mathf.Cos(elapsedTime)).normalized;
            gamepad.SetMotorSpeeds(vibrationValue * normalizedPosition.x, vibrationValue * normalizedPosition.y);
            yield return null;
        }
        gamepad.ResetHaptics();
    }

    private IEnumerator VibrateSpiral()
    {
        float startTime = Time.time;
        while (isVibrating)
        {
            float elapsedTime = Time.time - startTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / vibrationDuration);
            float vibrationValue = vibrationCurve.Evaluate(normalizedTime) * vibrationIntensity;

            Vector2 normalizedPosition = new Vector2(Mathf.Sin(elapsedTime * 3f), Mathf.Cos(elapsedTime * 3f)).normalized;
            gamepad.SetMotorSpeeds(vibrationValue * normalizedPosition.x, vibrationValue * normalizedPosition.y);
            yield return null;
        }
        gamepad.ResetHaptics();
    }

    private IEnumerator VibrateExplosion()
    {
        gamepad.SetMotorSpeeds(1f, 1f);
        yield return new WaitForSeconds(0.2f);
        gamepad.ResetHaptics();
    }
    private IEnumerator VibrateThrobbing()
    {
        float startTime = Time.time;
        float maxVibrationIntensity = vibrationIntensity * 0.8f;
        float pulseDuration = vibrationDuration / 2f;

        while (isVibrating)
        {
            float elapsedTime = Time.time - startTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / pulseDuration);

            // Incrementa la fuerza de vibración
            float vibrationValue = Mathf.Lerp(0f, maxVibrationIntensity, normalizedTime);
            gamepad.SetMotorSpeeds(vibrationValue, vibrationValue);

            if (normalizedTime >= 1f)
            {
                // Inicia la fase de disminución de la vibración
                float decreaseTime = elapsedTime - pulseDuration;
                float normalizedDecreaseTime = Mathf.Clamp01(decreaseTime / pulseDuration);

                // Disminuye la fuerza de vibración
                float decreaseVibrationValue = Mathf.Lerp(maxVibrationIntensity, 0f, normalizedDecreaseTime);
                gamepad.SetMotorSpeeds(decreaseVibrationValue, decreaseVibrationValue);

                if (normalizedDecreaseTime >= 1f)
                {
                    // Reinicia el temporizador y comienza un nuevo ciclo de vibración
                    startTime = Time.time;
                }
            }

            yield return null;
        }
        gamepad.ResetHaptics();
    }

}
