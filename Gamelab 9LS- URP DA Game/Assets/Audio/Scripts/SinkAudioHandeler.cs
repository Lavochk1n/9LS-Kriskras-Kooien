using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkAudioHandeler : AudioHandeler
{
    private SinkBehavior SB;
    [SerializeField] private AudioClip getGloves, washing;


   

    private void Awake()
    {
         SB = GetComponent<SinkBehavior>();
        audioSource = GetComponent<AudioSource>();
    }

    public void HandlesinkAudio()
    {
        audioSource.PlayOneShot(getGloves);
    }

    public void HandleHeldAudio()
    {
        if (audioSource.isPlaying) { return; }
        audioSource.PlayOneShot(washing);
    }


}
