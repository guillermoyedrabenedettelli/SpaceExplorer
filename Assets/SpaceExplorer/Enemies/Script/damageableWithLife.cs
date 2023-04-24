using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class damageableWithLife : MonoBehaviour, IDamageable
{
    [SerializeField] public float life;
    [SerializeField] public float life_dead = 0;
    bool alreadyDead = false;
    [SerializeField] public UnityEvent onDeath;
    [SerializeField] public UnityEvent<float> onChangeLife;

    AudioSource Hurt;
    bool principalPlayer = false;
    float timeSetMotor = 2;
    float timeCurrentSetMotor = 0;

    private void Awake()
    {
        life_dead = life;
        if (this.gameObject.name == "Player")
        {
            principalPlayer = true;
            if (Hurt != null) { Hurt = this.GetComponentInParent<AudioSource>(); }
        }
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
            }
            else
            {
                timeCurrentSetMotor = 0;
                Gamepad.current?.SetMotorSpeeds(0f, 0f);
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
                Hurt.Play();
            }
        }
        if (life_dead < 0f)
        {
            Destroy(gameObject);
            Gamepad.current?.SetMotorSpeeds(0f, 0f);
            alreadyDead = true;
            onDeath.Invoke();
        }
        if (life_dead > life)
        {
            life_dead = life;
        }
        print(life_dead);
    }
    UnityEvent IDamageable.GetDeathEvent()
    {
        return onDeath;
    }
}
