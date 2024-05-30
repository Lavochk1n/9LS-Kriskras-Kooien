using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AmbulanceTimer : MonoBehaviour
{
    private AmbulanceManager AM;

    [Header("Intervals")]
    private bool HasArrived = false;
    [SerializeField]
    private float
        awayTime = 30f,
        parkedTime = 8f;
    private float timeLeft;

    [Header("flickering")]
    //[SerializeField] private float flickerThreshold = 5f;
    private bool isFlickering = false;
    private float timerTotal;
    private float flickingInterval = 0.5f;
    [SerializeField] private Renderer greenLight, redLight;
    //private Renderer currentLight;

    private Material green, red; 
    [SerializeField] private Material white; 
    

    void Start()
    {
        AM = GetComponent<AmbulanceManager>();

        timeLeft = awayTime;
        timerTotal = timeLeft;

        green = greenLight.material; 
        red = redLight.material;

        redLight.material.EnableKeyword("_EMISSION");
        greenLight.material.DisableKeyword("_EMISSION");
    }

    public bool DecreaseTime()
    {

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        if (!isFlickering)
        {
            if (timeLeft < timerTotal / 4)
            {
                if(!HasArrived)
                {
                    StartCoroutine(FlickerLight(greenLight));
                    StartCoroutine(FlickerLight(redLight));

                }
            }
        }

        if (timeLeft < 0)
        {
            TurnOffFlickering();
            return true;
        }
        return false; 
    }

    public void TurnOffFlickering()
    {
        StopAllCoroutines();
        isFlickering = false;
        greenLight.material = green;
        redLight.material = red;
    }

    public void EngageLight(bool makeGreen)
    {
        if (makeGreen)
        {
            greenLight.material.EnableKeyword("_EMISSION");
            redLight.material.DisableKeyword("_EMISSION");
            //currentLight = greenLight;
        }
        else
        {
            greenLight.material.DisableKeyword("_EMISSION");
            redLight.material.EnableKeyword("_EMISSION");
            //currentLight = redLight;
        }
    }

    public void AddTime()
    {
        if(AM.HasArrived )
        {
            timeLeft = awayTime;
        }
        else
        {
            timeLeft = parkedTime;

        }
    timerTotal = timeLeft;
    }

    public float CurrentRelativeTime()
    {
        float time = 0;

        time = timeLeft / timerTotal;

        return time; 
    }

    private IEnumerator FlickerLight(Renderer light)
    {

        Material material = light.material;
        isFlickering = true;

        

        while (timeLeft > 0 && timeLeft < timerTotal)
        {
            light.material = white;
            light.material.EnableKeyword("_EMISSION");


            if (timeLeft < timerTotal / 8)
            {
                yield return new WaitForSeconds(flickingInterval / 4);

            }
            else yield return new WaitForSeconds(flickingInterval/2);

            light.material.DisableKeyword("_EMISSION");
            light.material = material;

            if (timeLeft < timerTotal / 8)
            {
                yield return new WaitForSeconds(flickingInterval / 2);

            }
            else yield return new WaitForSeconds(flickingInterval);
        }
        light.material = material;

        isFlickering = false;
    }
}
