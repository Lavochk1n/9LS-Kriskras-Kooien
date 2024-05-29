
using UnityEngine;
using UnityEngine.UI;


public class Interactor : MonoBehaviour
{    
    public GameObject interactionPrompt;
    [SerializeField] private float offset = -1f; 

    private bool requestInteract = false, requestHold = false, requestRemove = false;
    [SerializeField]  private Image interactionProgress;

    private GameObject currentPrompt; 

    /// <summary>
    /// ScanInteractable looks for objects with an Interactable class on it and sets its Interact() availible.   
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    /// <param name="raycastRange"></param>
    /// 
    public void ScanInteractable(GameObject origin, Vector3 direction = default, float raycastRange = default)
    {
        if (direction == default) direction = Vector3.forward;
        if (raycastRange == default) raycastRange = 10f;
    
        Debug.DrawRay(origin.transform.position, direction * raycastRange, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(origin.transform.position, direction, out hit, raycastRange))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            //if (hit.collider == null)
            //{
            //    requestHold = false;
            //}

            if (interactable != null)
            {
                HandleInteraction(interactable);

                if (interactable.interactionType == Interactable.InteractionType.Hold)
                {

                    Vector3 promptLocation = new Vector3
                       (
                        interactable.transform.position.x,
                        interactable.transform.position.y+ -offset,
                       interactable.transform.position.z);

                    if(currentPrompt == null)
                    {
                        currentPrompt = Instantiate(interactionPrompt, promptLocation, Quaternion.identity);
                        interactionProgress = currentPrompt.transform.Find("Ring").GetComponent<Image>(); 
                    }
                }
                else
                {
                    requestHold = false; 
                    if (currentPrompt != null)
                    {
                        Destroy(currentPrompt);
                        currentPrompt = null;
                        interactionProgress = null;
                    }
                }
            }
            else
            {
                requestHold = false;
                if (currentPrompt != null)
                {
                    Destroy(currentPrompt);
                    currentPrompt = null;
                    interactionProgress = null;
                }
            }
        }
        else
        {
            requestHold = false;
            if (currentPrompt != null)
            {
                Destroy(currentPrompt);
                currentPrompt = null;
                interactionProgress = null;
            }
        }
        

    }

    private void HandleInteraction(Interactable interactable)
    {
        interactable.GetDescription(); 

        switch (interactable.interactionType)
        {
            case Interactable.InteractionType.Click:
                if (requestInteract)
                {
                    interactable.Interact(this);
                    requestInteract = false;
                }

                if (requestRemove)
                {
                    interactable.Interact_Secondairy(this);
                }
                break;

            case Interactable.InteractionType.Hold:
                if (requestHold)
                {
                    interactable.IncreaseHoldTime(); 
                    if(interactable.GetHoldTime() > 1f)
                    {
                        interactable.Interact(this);
                        interactable.ResetHoldTime();
                    }
                interactionProgress.fillAmount = interactable.GetHoldTime();
                }
                break;

            default:
                throw new System.Exception("Unsupported type of interactable");
        }
    }

    public void RequestInteraction()
    {
        requestInteract = true;
        requestHold = true; 
    }


    public void RequestRemove()
    {
        requestRemove = true;
    }

    public void RequestHold()
    {
        requestHold = false;
    }

    private void LateUpdate()
    {
        if (requestInteract) { requestInteract = false; }
        if (requestRemove) { requestRemove = false; }

    }
}

    
