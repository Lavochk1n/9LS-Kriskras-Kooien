using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class ScoreDisplay : MonoBehaviour
{
     [SerializeField ]private TextMeshProUGUI scoreTextBox, roundTextBox;

    private string m_text;

    void Start()
    {
        scoreTextBox = GetComponentInChildren<TextMeshProUGUI>();  
       
        scoreTextBox.text = GameManager.Instance.GetScore().ToString();
        roundTextBox.text = GameManager.Instance.GetDepartures().ToString();


    }

}
