using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackThatBulance { 
    public class Item : Interactable
    {
        [SerializeField] private GameObject requiredSlot;

        private Vector3 startPos;
        private GameObject Cursor;

        private void Start()
        {
            Cursor = GameObject.FindGameObjectWithTag("Cursor"); 
            startPos = transform.position;
        }

        public override void Interact(Interactor interactor)
        {
            Cursor.GetComponent<CursorBehaviour>().GrabTarget(gameObject);
        }

        public override string GetDescription()
        {
            return "press 'A' to Interact"; 
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (requiredSlot == null) { return; }

            if (collision.gameObject == requiredSlot)
            {
                gameObject.SetActive(false);
                requiredSlot.GetComponent<Renderer>().material.color = Color.green;
            }

            if (collision.CompareTag("reset"))
            {
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.position = startPos;
            }
        }
    }
}

