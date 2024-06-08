using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowAnimation : MonoBehaviour
{
    private Image arrow;
    [SerializeField] private float bobtime = 2f;

    private float timer;

    private Vector3 ogPos; 

    // Start is called before the first frame update
    void Start()
    {
        ogPos = transform.position;
        arrow = GetComponentInChildren<Image>();

        timer = bobtime;
    }

    // Update is called once per frame
    void Update()
    {

        if (timer > bobtime) 
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer += Time.deltaTime; 
        }
        

        transform.position = ogPos + (Vector3.forward * timer);
        transform.position = arrow.transform.position;
    }
}
