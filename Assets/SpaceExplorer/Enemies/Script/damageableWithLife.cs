using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class damageableWithLife : MonoBehaviour, IDamageable
{
    [SerializeField] public float life;
    [SerializeField] public float life_dead = 0;
    bool alreadyDead = false;
    [SerializeField] public UnityEvent onDeath;
    [SerializeField] public UnityEvent<float> onChangeLife;

    [Header("Droop Configurations")]
    [SerializeField] GameObject[] DropeableItems;
    [SerializeField] int NoDropChance=100;

    int range;

    [SerializeField] Image healthBar;

    AudioSource Hurt;
    bool principalPlayer = false;
    float timeSetMotor = 2;
    float timeCurrentSetMotor = 0;

    private void Awake()
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
