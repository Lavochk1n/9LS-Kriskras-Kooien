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

    // Sorry felix, I put this function in to make it work. 
    public override void Interact_Secondairy(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }

    public override string GetDescription()
    {
        return null;
    }
    
}
