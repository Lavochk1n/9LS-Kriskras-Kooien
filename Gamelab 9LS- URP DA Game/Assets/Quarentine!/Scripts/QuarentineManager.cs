using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuarentineManager : MonoBehaviour
{
    
    public static QuarentineManager Instance { get; private set; }

    [Header("colours")]

    public Material dog;
    public Material native,exotic,empty,closed;

    public float spreadSpeed;



    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }




}
