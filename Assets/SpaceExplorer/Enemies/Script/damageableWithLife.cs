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
    protected bool alreadyDead = false;
    public UnityEvent onDeath;
    public UnityEvent<float> onChangeLife;

    [Header("Droop Configurations")]
    [SerializeField] GameObject[] DropeableItems;
    [SerializeField] int NoDropChance = 100;


    int range;

    [SerializeField] protected Image healthBar;

    AudioSource Hurt;
    bool principalPlayer = false;
    float timeSetMotor = 2;
    protected float timeCurrentSetMotor = 0;

    private void Awake()
    {
        
        baseAwake();
        foreach (GameObject item in DropeableItems)
        {
            DropeableItem dropeable = item.GetComponentInChildren<DropeableItem>();
            range += dropeable.GetDropRate();
        }
        range = range + NoDropChance;
        
    }

    public void baseAwake()
    {
        life_dead = life;
        if (this.gameObject.GetComponent<CharacterController>())
        {
            principalPlayer = true;
            if (Hurt != null) { Hurt = this.GetComponentInParent<AudioSource>(); }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
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
            onDamageableDies();
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

    virtual protected void onDamageableDies()
    {
        SpawnDrop();

        Destroy(gameObject);
        Gamepad.current?.SetMotorSpeeds(0f, 0f);
        alreadyDead = true;
        onDeath.Invoke();
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

