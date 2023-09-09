using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Manage : MonoBehaviour
{
    [SerializeField] AudioSource FowardSound;
    [SerializeField] AudioSource BackSound;
    [SerializeField] AudioSource LeftSound;
    [SerializeField] AudioSource RigthSound;
    [SerializeField] AudioSource TakeOffSound;

    // Start is called before the first frame update
    void Start()
    {
        if (FowardSound != null) FowardSound = GetComponent<AudioSource>();
        if(BackSound != null) BackSound = GetComponent<AudioSource>();
        if(LeftSound != null) LeftSound = GetComponent<AudioSource>();
        if(RigthSound != null) RigthSound = GetComponent<AudioSource>();
        if(TakeOffSound != null) TakeOffSound = GetComponent<AudioSource>();
    }
    public void PlayFoward() { FowardSound.Play(); }
    public void PlayBackSound() { BackSound.Play(); }
    public void PlayLeftSound() { LeftSound.Play(); }
    public void PlayRight() { RigthSound.Play(); }
    public void PlayTakeOff() { TakeOffSound.Play(); }
}
