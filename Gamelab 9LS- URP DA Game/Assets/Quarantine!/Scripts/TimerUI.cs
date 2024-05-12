using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{

    private float TotalTime, TimeLeft;

    [Header("Timer")]
    private Image timer;
    [SerializeField] private Gradient timercolour;

    void Start()
    {
        TotalTime = AmbulanceManager.Instance.GetTotalGameTime();
        timer = GetComponent<Image>();  
    }


    void Update()
    {
        TotalTime = AmbulanceManager.Instance.GetTotalGameTime();
        TimeLeft = TotalTime - AmbulanceManager.Instance.GetTimeLeft();

        timer.fillAmount = 1f - (TimeLeft / TotalTime);
        timer.color = timercolour.Evaluate((1f - TimeLeft / TotalTime));
    }
}
