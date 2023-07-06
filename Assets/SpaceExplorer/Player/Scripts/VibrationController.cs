using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;




public class VibrationController : MonoBehaviour
{
    
    [SerializeField] float vibrationDuration = 1f;
    [SerializeField] float vibrationIntensity = 0.5f;
    [SerializeField] AnimationCurve vibrationCurve;
    public bool Active = false;

    private Gamepad gamepad;
    private bool isVibrating = false;
    private VibrationSense sense;
    private void Start()
    {
        // Obt�n el gamepad principal
        gamepad = Gamepad.current;
    }

    private void Update()
    {
        if (gamepad != null && Active == true)
        {
            if (!isVibrating)
            {
                // Comienza una subrutina de vibraci�n aleatoria
                isVibrating = true;

                int vibrationIndex = Random.Range(0, 8);
                switch (sense)
                {
                    case global::VibrationSense.FastPulse:
                        StartCoroutine(VibrateFastPulse());
                        Debug.Log("Reproduciendo: R�faga r�pida");
                        break;
                    case global::VibrationSense.SlowPulse:
                        StartCoroutine(VibrateSlowPulse());
                        Debug.Log("Reproduciendo: Pulso tranquilo");
                        break;
                    case global::VibrationSense.AscendingBurst:
                        StartCoroutine(VibrateAscendingBurst());
                        Debug.Log("Reproduciendo: R�faga ascendente");
                        break;
                    case global::VibrationSense.IrregularPattern:
                        StartCoroutine(VibrateIrregularPattern());
                        Debug.Log("Reproduciendo: Vibraci�n irregular");
                        break;
                    case global::VibrationSense.ExpandingWave:
                        StartCoroutine(VibrateExpandingWave());
                        Debug.Log("Reproduciendo: Onda expansiva");
                        break;
                    case global::VibrationSense.Spiral:
                        StartCoroutine(VibrateSpiral());
                        Debug.Log("Reproduciendo: Vibraci�n en espiral");
                        break;
                    case global::VibrationSense.Explosion:
                        StartCoroutine(VibrateExplosion());
                        Debug.Log("Reproduciendo: Vibraci�n de explosi�n");
                        break;
                    case global::VibrationSense.Throbbing:
                        StartCoroutine(VibrateThrobbing());
                        Debug.Log("Reproduciendo: Vibraci�n de Throbbing");
                        break;
                }
            }
        }
        else if (isVibrating)
        {
            // Detiene la vibraci�n
            isVibrating = false;
            Active = false;
            gamepad.ResetHaptics();
            Debug.Log("Deteniendo vibraci�n");
        }
    }
    public void VibrationSense(VibrationSense number,bool Ac)
    {
        sense = number;
        Active = Ac;
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

            // Incrementa la fuerza de vibraci�n
            float vibrationValue = Mathf.Lerp(0f, maxVibrationIntensity, normalizedTime);
            gamepad.SetMotorSpeeds(vibrationValue, vibrationValue);

            if (normalizedTime >= 1f)
            {
                // Inicia la fase de disminuci�n de la vibraci�n
                float decreaseTime = elapsedTime - pulseDuration;
                float normalizedDecreaseTime = Mathf.Clamp01(decreaseTime / pulseDuration);

                // Disminuye la fuerza de vibraci�n
                float decreaseVibrationValue = Mathf.Lerp(maxVibrationIntensity, 0f, normalizedDecreaseTime);
                gamepad.SetMotorSpeeds(decreaseVibrationValue, decreaseVibrationValue);

                if (normalizedDecreaseTime >= 1f)
                {
                    // Reinicia el temporizador y comienza un nuevo ciclo de vibraci�n
                    startTime = Time.time;
                }
            }

            yield return null;
        }
        gamepad.ResetHaptics();
    }

}
