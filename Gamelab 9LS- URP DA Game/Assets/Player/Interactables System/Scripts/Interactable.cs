using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        Click,
        Hold,
        Minigame,
    }

    bool heldDown;

    float holdTime;

    public InteractionType interactionType;

    public abstract string GetDescription();

    public abstract void Interact(Interactor interactor);

    public abstract void Interact_Secondairy(Interactor interactor);


    public void IncreaseHoldTime()
    {     
    holdTime += Time.deltaTime;
    heldDown = true;
    }

public void ResetHoldTime() => holdTime = 0f;
    public float GetHoldTime() => holdTime;

    private void LateUpdate()
    {
        heldDown = false;
    }

    public bool Isheld()
    {
        return heldDown;
    }
}
