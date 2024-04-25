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

    // Start is called before the first frame update
    void Start()
    {
        TotalTime = GameManager.Instance.GetTotalGameTime();
        timer = GetComponent<Image>();  
    }


    // Update is called once per frame
    void Update()
    {
        TotalTime = GameManager.Instance.GetTotalGameTime();
        TimeLeft = TotalTime - GameManager.Instance.GetTimeLeft();

        timer.fillAmount = 1f - (TimeLeft / TotalTime);
        timer.color = timercolour.Evaluate((1f - TimeLeft / TotalTime));
    }
}
