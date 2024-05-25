using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        AM = GetComponent<AmbulanceManager>();

        timeLeft = awayTime;
        timerTotal = timeLeft;

        redLight.material.EnableKeyword("_EMISSION");
        greenLight.material.DisableKeyword("_EMISSION");
    }

    public bool DecreaseTime()
    {
        Debug.Log(timeLeft);

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
                }
            }
        }

        if (timeLeft < 0)
        {
            StopAllCoroutines();
            isFlickering = false;
            return true;
        }
        return false; 
    }

    public void AddTime( )
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

    private IEnumerator FlickerLight(Renderer light)
    {
        isFlickering = true;

        while (true)
        {
            light.material.EnableKeyword("_EMISSION");

            if (timeLeft < timerTotal / 8)
            {
                yield return new WaitForSeconds(flickingInterval / 2);

            }
            else yield return new WaitForSeconds(flickingInterval);


            light.material.DisableKeyword("_EMISSION");

            if (timeLeft < timerTotal / 8)
            {
                yield return new WaitForSeconds(flickingInterval / 2);

            }
            else yield return new WaitForSeconds(flickingInterval);
        }
    }
}
