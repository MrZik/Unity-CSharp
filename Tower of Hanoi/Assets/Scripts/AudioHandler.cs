using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler Instance { get; private set; }
    [SerializeField] AudioSource Source;

    private void Awake()
    {
        Singleton();

        if(Source == null)
        {
            Source = GetComponent<AudioSource>();
        }
    }

    private void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    public void PlayAudio(AudioClip clip)
    {
        Source.PlayOneShot(clip);
    }
}
