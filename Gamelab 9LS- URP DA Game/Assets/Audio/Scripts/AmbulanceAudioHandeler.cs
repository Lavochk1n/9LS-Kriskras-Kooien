using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbulanceAudioHandeler : AudioHandeler
{
    private AmbulanceManager AM; 

    void Start()
    {
      AM = GetComponent<AmbulanceManager>();  
    }

    
}
