using Quarantine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundStatisticsDisplay : MonoBehaviour
{


    [SerializeField]private TextMeshProUGUI score,sickCages,quotaCages, rounds;



    public void RefreshStats(int amount, int quota)
    {
        score.text = GameManager.Instance.GetScore().ToString();

        quotaCages.text = "/ " + quota.ToString();

        sickCages.text = amount.ToString();
        
        rounds.text = "#" +GameManager.Instance.GetDepartures().ToString();

    }

}
