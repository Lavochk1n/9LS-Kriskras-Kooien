using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkBehavior : Interactable
{
    private GloveManager glovemanager;

    public override void Interact(Interactor interactor)
    {
        glovemanager = interactor.GetComponent<GloveManager>();
        glovemanager.AddGloves();

    }

    public override string GetDescription()
    {
        return null;
    }
    
}
