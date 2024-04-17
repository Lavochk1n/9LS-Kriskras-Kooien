using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackThatBulance
{
    public class Item : Interactable
    {
        [SerializeField] private Transform requiredSlot; // Change type to Transform

        private Vector3 startPos;
        private GameObject Cursor;
        private bool isBeingHeld = false; // Flag to track if the item is being held by the player

        private void Start()
        {
            Cursor = GameObject.FindGameObjectWithTag("Cursor");
            startPos = transform.position;
        }

        public override void Interact(Interactor interactor)
        {
            if (!isBeingHeld)
            {
                Cursor.GetComponent<CursorBehaviour>().GrabTarget(gameObject);
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

        private void OnTriggerEnter(Collider collision)
        {
            if (requiredSlot == null) { return; }

            if (collision.gameObject == requiredSlot.gameObject && isBeingHeld) // Check if being held and collides with slot
            {
                SnapToSlot();
            }

            if (collision.CompareTag("reset"))
            {
                if (isBeingHeld) // Reset only if the item is being held
                {
                    ResetItem();
                }
            }
        }

        // Function to snap the item to the slot
        private void SnapToSlot()
        {
            transform.position = requiredSlot.position; // Snap to the required slot's position
            GetComponent<Rigidbody>().isKinematic = true; // Disable physics
            isBeingHeld = false; // Release the item
        }

        // Function to release the item
        private void ReleaseItem()
        {
            Cursor.GetComponent<CursorBehaviour>().GrabTarget(null); // Release the item from the cursor
            GetComponent<Rigidbody>().isKinematic = false; // Enable physics
            isBeingHeld = false; // Release the item
        }

        // Function to reset the item to its original position
        private void ResetItem()
        {
            transform.position = startPos;
            GetComponent<Rigidbody>().velocity = Vector3.zero; // Reset velocity
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // Reset angular velocity
        }
    }
}





