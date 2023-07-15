using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class damageableWithLife : MonoBehaviour, IDamageable
{
    public float life;
    public float life_dead = 0;
    bool alreadyDead = false;
    public UnityEvent onDeath;
    public UnityEvent<float> onChangeLife;

    [Header("Droop Configurations")]
    [SerializeField] GameObject[] DropeableItems;
    [SerializeField] int NoDropChance = 100;

    private Gamepad gamepad;
    float vibrationDuration = 1f;
    float vibrationIntensity = 0.5f;
    AnimationCurve vibrationCurve;

    int range;

    [SerializeField] protected Image healthBar;

    AudioSource Hurt;
    bool principalPlayer = false;
    float timeSetMotor = 2;
    float timeCurrentSetMotor = 0;

    private void Awake()
    {
        vibrationCurve = new AnimationCurve();
        // Agregamos puntos clave (keyframes) al curva
        vibrationCurve.AddKey(0f, 0f); // En el tiempo 0, el valor es 0
        vibrationCurve.AddKey(1f, 0.5f); // En el tiempo 1, el valor es 1
        vibrationCurve.AddKey(2f, 1f); // En el tiempo 2, el valor es 0.
        vibrationCurve.SmoothTangents(1, 0f);
        baseAwake();
    }

    public void baseAwake()
    {
        life_dead = life;
        if (this.gameObject.GetComponent<CharacterController>())
        {
            principalPlayer = true;
            if (Hurt != null) { Hurt = this.GetComponentInParent<AudioSource>(); }
        }
        if (!principalPlayer)
        {
            foreach (GameObject item in DropeableItems)
            {
                DropeableItem dropeable = item.GetComponentInChildren<DropeableItem>();
                range += dropeable.GetDropRate();
            }
            range = range + NoDropChance;
        }
    }


    private IEnumerator spiral()
    {
        float startTime = Time.time;

        float elapsedTime = Time.time - startTime;
        float normalizedTime = Mathf.Clamp01(elapsedTime / vibrationDuration);
        float vibrationValue = vibrationCurve.Evaluate(normalizedTime) * vibrationIntensity;

        Vector2 normalizedPosition = new Vector2(Mathf.Sin(elapsedTime * 3f), Mathf.Cos(elapsedTime * 3f)).normalized;
        gamepad.SetMotorSpeeds(vibrationValue * normalizedPosition.x, vibrationValue * normalizedPosition.y);
        yield return null;

        gamepad.ResetHaptics();
    }

    // Update is called once per frame
    void Update()
    {
        if (principalPlayer)
        {
            if (timeCurrentSetMotor >= 1)
            {
                timeCurrentSetMotor -= Time.deltaTime;
                Gamepad.current?.SetMotorSpeeds(0.25f, 0.75f);
                //this.gameObject.GetComponent<CameraShaker>().StartShake(1, .1f);
            }
            else
            {
                timeCurrentSetMotor = 0;
                Gamepad.current?.SetMotorSpeeds(0f, 0f);
            }
            if(life_dead < (life / 2)){
                StartCoroutine(spiral());
            }

        }

    }
    void IDamageable.NotifyHit(float damage)
    {
        life_dead -= damage;
        onChangeLife.Invoke(life_dead);
        if (principalPlayer)
        {
            if (damage >= 1f)
            {
                timeCurrentSetMotor = timeSetMotor;
                if (Hurt != null)
                {
                    Hurt.Play();
                }
            }
        }
        if (healthBar != null)
        {
            healthBar.fillAmount = life_dead / life;
        }
        if (life_dead < 0.5f)
        {
            if (!principalPlayer)
            {
                SpawnDrop();

            }
            else
            {

                SceneManager.LoadScene("Test");
            }
            Destroy(gameObject);
            Gamepad.current?.SetMotorSpeeds(0f, 0f);
            alreadyDead = true;
            onDeath.Invoke();
        }
        if (life_dead > life)
        {
            life_dead = life;
        }
        //print(life_dead);
    }
    UnityEvent IDamageable.GetDeathEvent()
    {
        return onDeath;
    }

    void SpawnDrop()
    {
        int dropNumber = Random.Range(0, range);
        if (dropNumber > NoDropChance)
        {
            float loopNumber = NoDropChance;
            bool notfound;
            if (DropeableItems.Length == 0)
            {
                notfound = false;
            }
            else
            {
                notfound = true;
            }

            int i = 0;
            GameObject SpawnObject = null;
            while (notfound)
            {
                DropeableItem dropeable = DropeableItems[i].GetComponentInChildren<DropeableItem>();
                float currentDropRate = dropeable.GetDropRate();
                loopNumber += currentDropRate;
                if (dropNumber <= loopNumber)
                {
                    notfound = false;
                    SpawnObject = DropeableItems[i];
                }
                else
                {
                    i++;
                }
            }
            if (SpawnObject != null)
            {
                Instantiate(SpawnObject, transform.position, transform.rotation);
            }
        }
    }

}

