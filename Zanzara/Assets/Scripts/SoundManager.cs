using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClip SuckingSound;
    public AudioSource audioSource;
    private bool isSuckingSoundPlaying = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySuckingSound()
    {
        if (!isSuckingSoundPlaying)
        {
            audioSource.PlayOneShot(SuckingSound);
            isSuckingSoundPlaying = true;
        }
    }
     public void StopSuckingSound()
    {
        if (isSuckingSoundPlaying)
        {
            audioSource.Stop();
            isSuckingSoundPlaying = false;
        }
    }
    
}
