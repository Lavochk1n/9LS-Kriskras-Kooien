using UnityEngine;

namespace StackThatBulance
{
    public class Item : Interactable
    {
        private Rigidbody rb;
        private bool isBeingHeld = false; 
        private bool isInBox = false; 

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
                // GameManager.Instance.UpdateScore(); // Update the score when the item enters the box
                CheckIfAllItemsInBox();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Box"))
            {
                isInBox = false;
            }
        }
        private void CheckIfAllItemsInBox()
        {
            GameObject[] itemsInBox = GameObject.FindGameObjectsWithTag("Item");
            int totalItems = 7;
            int itemsCount = 0;

            foreach (GameObject item in itemsInBox)
            {
                Item itemComponent = item.GetComponent<Item>();
                if (itemComponent != null && itemComponent.isInBox)
                {
                    itemsCount++;
                }
            }

            if (itemsCount == totalItems)
            {
                GameManager.Instance.FinishGame();
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







