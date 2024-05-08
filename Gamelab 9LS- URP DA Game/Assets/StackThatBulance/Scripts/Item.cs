using UnityEngine;

namespace StackThatBulance
{
    public class Item : Interactable
    {
        private Rigidbody rb;
        private bool isBeingHeld = false; 
        public bool isInBox = false;
        public float pointValue = 10f; 

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public override void Interact(Interactor interactor)
        {
            if (!isBeingHeld)
            {
                CursorBehaviour cursor = GameObject.FindWithTag("Cursor").GetComponent<CursorBehaviour>();
                cursor.GrabTarget(gameObject);
                isBeingHeld = true;
            }
            else
            {
                ReleaseItem();
            }
        }
        public override void Interact_Secondairy(Interactor interactor)
        {
            throw new System.NotImplementedException();
        }

        public override string GetDescription()
        {
            if (!isBeingHeld)
            {
                return "press 'A' to Interact";
            }
            else
            {
                return "press 'A' to Drop";
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Box"))
            {
                isInBox = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Box"))
            {
                isInBox = false;
            }
        }
        

        private void ReleaseItem()
        {
            CursorBehaviour cursor = FindObjectOfType<CursorBehaviour>();
            if (cursor != null)
            {
                cursor.GrabTarget(null); 
            }

            isBeingHeld = false; 
            rb.useGravity = true; 
            rb.isKinematic = false; 
        }
    }
}







