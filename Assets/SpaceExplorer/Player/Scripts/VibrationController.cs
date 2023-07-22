using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;




public class VibrationController : MonoBehaviour
{

    [SerializeField] float vibrationDuration = 1f;
    [SerializeField] float vibrationIntensity = 0.5f;
    [SerializeField] AnimationCurve vibrationCurve;
    public GameObject SpaceShip;

    public float startTime = 0f;
    public float endTime = 1f;
    public int numSamples = 100000;


    public bool Active = false;

    private Gamepad gamepad;
    private bool isVibrating = false;
    private VibrationSense sense;
    Coroutine coroutine = null;

    private float Sigmoid(float x)
    {
        return 1 / (1 + Mathf.Exp(-x));
    }

    private void Start()
    {
        // Obtén el gamepad principal
        gamepad = Gamepad.current;
        vibrationCurve = new AnimationCurve();
        for (int i = 0; i <= numSamples; i++)
        {
            float time = Mathf.Lerp(startTime, endTime, (float)i / numSamples);
            float value = Sigmoid(time);

            Keyframe keyframe = new Keyframe(time, value);
            vibrationCurve.AddKey(keyframe);
        }
    }

    private void Update()
    {
        
        if (gamepad != null && Active == true)
            {
                if (!isVibrating)
                {
                    // Comienza una subrutina de vibración aleatoria
                    isVibrating = true;

                    int vibrationIndex = Random.Range(0, 8);
                    switch (sense)
                    {
                        case global::VibrationSense.FastPulse:
                            coroutine = StartCoroutine(VibrateFastPulse());
                            Debug.Log("Reproduciendo: Ráfaga rápida");
                            break;
                        case global::VibrationSense.SlowPulse:
                            coroutine = StartCoroutine(VibrateSlowPulse());
                            Debug.Log("Reproduciendo: Pulso tranquilo");
                            break;
                        case global::VibrationSense.AscendingBurst:
                            coroutine = StartCoroutine(VibrateAscendingBurst());
                            Debug.Log("Reproduciendo: Ráfaga ascendente");
                            break;
                        case global::VibrationSense.IrregularPattern:
                            coroutine = StartCoroutine(VibrateIrregularPattern());
                            Debug.Log("Reproduciendo: Vibración irregular");
                            break;
                        case global::VibrationSense.ExpandingWave:
                            coroutine = StartCoroutine(VibrateExpandingWave());
                            Debug.Log("Reproduciendo: Onda expansiva");
                            break;
                        case global::VibrationSense.Spiral:
                            coroutine = StartCoroutine(VibrateSpiral());
                            Debug.Log("Reproduciendo: Vibración en espiral");
                            break;
                        case global::VibrationSense.Explosion:
                            coroutine = StartCoroutine(VibrateExplosion());
                            Debug.Log("Reproduciendo: Vibración de explosión");
                            break;
                        case global::VibrationSense.Throbbing:
                            coroutine = StartCoroutine(VibrateThrobbing());
                            Debug.Log("Reproduciendo: Vibración de Throbbing");
                            break;
                    }
                }
            }
            else if (isVibrating)
            {
                // Detiene la vibración
                isVibrating = false;
                Active = false;
                gamepad.ResetHaptics();
                Debug.Log("Deteniendo vibración");
                //StopCoroutine(reset());
            }
        
    }

    private IEnumerator reset()
    {
        yield break;
        
    }


    public void VibrationSense(VibrationSense number, bool Ac)
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
