using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageAudioHandeler : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioManager AM;
    private CageBehaviour CB;

    [SerializeField] private AudioClip[] rightFoot, leftFoot;
    [SerializeField] private AudioClip dash;

    [SerializeField] private float stepsTime = .1f;



    void Awake()
    {
        AM = AudioManager.Instance;
        CB = GetComponent<CageBehaviour>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = AM.GlobalVolume();
    }
}
