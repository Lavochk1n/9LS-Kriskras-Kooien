using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandeler : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioManager AM;

    void Awake()
    {
        AM = AudioManager.Instance;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = AM.GlobalVolume();
    }



}
