using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private bool requestInteract = false;

    /// <summary>
    /// ScanInteractable looks for objects with an Interactable class on it and sets its Interact() availible.   
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    /// <param name="raycastRange"></param>
    public void ScanInteractable(GameObject origin, Vector3 direction = default, float raycastRange = default)
    {
        if (direction == default) direction = Vector3.forward;
        if (raycastRange == default) raycastRange = 10f;
        
        Debug.DrawRay(origin.transform.position, direction * raycastRange, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(origin.transform.position, direction, out hit, raycastRange))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                HandleInteraction(interactable);
            }
        }
    }

    private void HandleInteraction(Interactable interactable)
    {
        if (requestInteract /*|| holding*/)
        {
            switch (interactable.interactionType)
            {
                case Interactable.InteractionType.Click:
                    interactable.Interact(this);
                    requestInteract = false;
                    break;
                default:
                    throw new System.Exception("Unsupported type of interactable");
            }
        }
    }

    public void RequestInteraction()
    {
        requestInteract = true;
    }

    private void LateUpdate()
    {
        if (!requestInteract) { return; }
        requestInteract = false ;
    }
}

    
