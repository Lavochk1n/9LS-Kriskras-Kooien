using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SinkBehavior : Interactable
{
    private GloveManager glovemanager;

    public override void Interact(Interactor interactor)
    {
        glovemanager = interactor.GetComponent<GloveManager>();
        glovemanager.AddGloves();
        GetComponent<SinkAudioHandeler>().HandlesinkAudio(); 
        

    }


    public override string GetDescription()
    {
        if (Isheld())
        {
            GetComponent<SinkAudioHandeler>().HandleHeldAudio();
        }
        return null; 
    }
    
}
