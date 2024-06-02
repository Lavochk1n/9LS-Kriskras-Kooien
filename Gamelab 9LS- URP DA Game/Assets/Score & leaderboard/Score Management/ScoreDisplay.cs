using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class ScoreDisplay : MonoBehaviour
{
     private TextMeshProUGUI textBox;

    private string m_text;

    void Start()
    {
        textBox = GetComponentInChildren<TextMeshProUGUI>();  
        m_text =  GameManager.Instance.GetScore().ToString(); 
        
    }

    private void Update()
    {
        m_text = GameManager.Instance.GetScore().ToString();
        textBox.text = m_text;
    }
}
