using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class FloatText : MonoBehaviour
{
 


    [SerializeField] private float textTime =  1;

    private TextMeshProUGUI text; 

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        transform.LookAt(GameObject.FindWithTag("MainCamera").transform);
    }

    private void Update()
    {
        textTime -= Time.deltaTime;
        text.CrossFadeAlpha(0f, textTime, true);

        if (textTime < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetScore(int score)
    {
        string prefix = "";
        Color color = Color.white;

        if (score > 0)
        {
            prefix = "+"; color = Color.green;
        }
        if (score < 0)
        {
            color = Color.red;
        }
        text.text = prefix + score.ToString();
        text.color = color;
    }    
}
