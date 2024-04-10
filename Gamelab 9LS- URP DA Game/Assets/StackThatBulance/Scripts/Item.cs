using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackThatBulance
{
    public class Item : Interactable
    {
        [SerializeField] private Transform requiredSlot;

        private Vector3 startPos;
        private GameObject Cursor;

        private bool isPlaced = false;

        private void Start()
        {
            Cursor = GameObject.FindGameObjectWithTag("Cursor");
            startPos = transform.position;
        }

        public override void Interact()
        {
            if (!isPlaced)
                Cursor.GetComponent<CursorBehaviour>().GrabTarget(gameObject);
        }

        public override string GetDescription()
        {
            return "Press 'A' to Interact";
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (requiredSlot == null || isPlaced)
                return;

            if (collision.transform == requiredSlot)
            {
                transform.position = requiredSlot.position;
                isPlaced = true;
                requiredSlot.GetComponent<Renderer>().material.color = Color.green;
                // Optionally, you can disable the collider of the item once it's placed to prevent further interactions.
                GetComponent<Collider2D>().enabled = false;
            }

            if (collision.CompareTag("reset"))
            {
                GetComponent<Rigidbody2D>().gravityScale = 0;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                transform.position = startPos;
            }
        }
    }
}


