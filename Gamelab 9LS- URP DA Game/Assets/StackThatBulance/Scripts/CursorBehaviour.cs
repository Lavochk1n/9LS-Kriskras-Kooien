using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
namespace StackThatBulance
{
    public class CursorBehaviour : MonoBehaviour
    {
        public Vector3 moveVector = Vector3.zero;
        private Rigidbody rb;

        [SerializeField] private float movespeed = 10f;

        private GameObject grabTarget;

        [SerializeField] private Transform grapPoint; 

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

        }

        private void FixedUpdate()
        {
            rb.velocity = moveVector * movespeed;

            if (grabTarget != null)
            {
                grabTarget.transform.position = grapPoint.transform.position;
            }
        }

        public void GrabTarget(GameObject target)
        {
            if (grabTarget == null) 
            {
                grabTarget = target;
            }
            else
            {
                grabTarget.GetComponent<Rigidbody>().useGravity = true;
                grabTarget = null;
            }

        }


    }
}