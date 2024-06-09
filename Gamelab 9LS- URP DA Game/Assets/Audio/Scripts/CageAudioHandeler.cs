using Quarantine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CageAudioHandeler : AudioHandeler
{
    private CageBehaviour CB;

    [SerializeField] private AudioClip[] sickClips, HealthyClips;
    [SerializeField] private AudioClip CageRattle;

    private float cooldown = .2f; 
    private float timer;

    private AudioSource audioSourceRattle; 
    void Start()
    {
        CB = GetComponent<CageBehaviour>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSourceRattle = gameObject.AddComponent<AudioSource>();

        audioSource.volume = AM.GlobalVolume();
        audioSourceRattle.volume = AM.GlobalVolume()/2f;
    }

    private void Update()
    {
        if (timer < 0) return; 
        if (audioSource.isPlaying) return;
        timer -= Time.deltaTime;
    }



    public void HandleSickSound(AnimalTypes type)
    {

        switch (type)
        {
            case AnimalTypes.Bunny:
                audioSource.PlayOneShot(sickClips[0]);
                break;
            case AnimalTypes.parrot:
                audioSource.PlayOneShot(sickClips[1]);

                break;
            case AnimalTypes.crow:
                audioSource.PlayOneShot(sickClips[2]);

                break;
            default: Debug.LogError("Uknown type"); break; 
        }
        

    }
        

    public void HandleInteractionSound(Animal animal)
    {
        if (timer > 0) return;

        switch (animal.type)
        {
            case AnimalTypes.Bunny:

                if (animal.state == SickState.sick) audioSource.PlayOneShot(sickClips[0]);
                else audioSource.PlayOneShot(HealthyClips[0]);
                break;
            case AnimalTypes.parrot:
                if (animal.state == SickState.sick) audioSource.PlayOneShot(sickClips[1]);
                else audioSource.PlayOneShot(HealthyClips[1]);
                break;
            case AnimalTypes.crow:
                if (animal.state == SickState.sick) audioSource.PlayOneShot(sickClips[2]);
                else audioSource.PlayOneShot(HealthyClips[2]);
                break;
            default: Debug.LogError("Uknown type"); break;
        }
        
        audioSourceRattle.PlayOneShot(CageRattle); 
        timer = cooldown;

    }

    [System.Serializable]
    private class AnimalAudio
    {

        


    }

}
