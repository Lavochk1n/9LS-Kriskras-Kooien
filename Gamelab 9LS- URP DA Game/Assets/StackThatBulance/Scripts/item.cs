using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackThatBulance
{
    public class item : MonoBehaviour
    {

        [SerializeField] private GameObject requiredSlot;
        private Vector3 startPos;

        private void Start()
        {
            startPos = transform.position;
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