using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioHandeler : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioManager AM;
    private PlayerBehaviour PB;

    [SerializeField] private AudioClip[] rightFoot, leftFoot;
    [SerializeField] private AudioClip dash;

    [SerializeField] private float stepsTime = .1f;



    void Awake()
    {
        AM = AudioManager.Instance;
        PB = GetComponent<PlayerBehaviour>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = AM.GlobalVolume();
    }
    private void Update()
    {
        if (isDashing) return;

        if (PB.SprintSpeed() > 1)
        {
            StopAllCoroutines();
            StartCoroutine(HandleDash());
            isWalking = false;

        }
        

        if (PB.IsMoving())
        {
            if (!isWalking)
            {
                StartCoroutine(HandleFootsteps());
            }
        }
        else
        {
            StopAllCoroutines();
            isWalking = false;  
        }

    }

    private bool isWalking; 
    private IEnumerator HandleFootsteps()
    {
        isWalking = true;
        while (true)
        {
            AudioClip clip = rightFoot[Random.Range(0, rightFoot.Length)];
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(stepsTime);

            clip = leftFoot[Random.Range(0, leftFoot.Length)];
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(stepsTime);

        }

    }

    private bool isDashing = false; 
    private IEnumerator HandleDash()
    {
        isDashing = true;
        audioSource.PlayOneShot(dash);

        yield return new WaitForSeconds(1f);
        isDashing = false;


    }

}
