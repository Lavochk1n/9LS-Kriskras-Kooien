using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource,sfxSource;

    [SerializeField] private AudioClip[] musicClips,sfxClips;

    [SerializeField] float globalVolume = 1f; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //PlayMusic(0);
    }


    public void PlayMusic(int trackIndex)
    {
        if (trackIndex < 0 || trackIndex >= musicClips.Length) return;

        musicSource.clip = musicClips[trackIndex];
        musicSource.Play();
    }

    public void PlaySFX(int sfxIndex, AudioSource source = default)
    {

        if (sfxIndex < 0 || sfxIndex >= sfxClips.Length) return;

        if (source == default)
        {
            source = sfxSource;
        }
        source.volume = globalVolume;

        source.PlayOneShot(sfxClips[sfxIndex]);
    }


    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume * globalVolume;
    }

    public void SetVolume(float volume)
    {
        globalVolume = volume;
    }

    public void MuteAllAudio(bool isMuted)
    {
        AudioListener.volume = isMuted ? 0 : 1;
    }

    public float GlobalVolume()
    {
        return globalVolume;
    }

}
