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

        [SerializeField] private float movespeed = 10f, cooldownTime = .01f;

        private float currentCooldown = 0f; 

        private GameObject GrapTarget;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

        }



        private void FixedUpdate()
        {
            rb.velocity = moveVector * movespeed;

            if (GrapTarget != null)
            {
                GrapTarget.transform.position = this.transform.position;
            }

            if (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
                if (currentCooldown < 0)
                {
                    currentCooldown = 0; 
                }
            }
        

        }


        public void AttemptGrab()
        {
            if(currentCooldown > 0)
            {
                return;
            }

            currentCooldown = cooldownTime;

            if (GrapTarget == null)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, Vector3.down, out hit))
                {
                    Debug.Log(hit.transform.gameObject);
                    GrapTarget = hit.transform.gameObject;

                    if (GrapTarget.GetComponent<Rigidbody>() == null) { GrapTarget = null; return; }

                    GrapTarget.GetComponent<Rigidbody>().useGravity = false;
                }
            }
            else
            {
                Debug.Log("Let Go");
                GrapTarget.GetComponent<Rigidbody>().useGravity = true;
                GrapTarget = null;

            }
        }

    }
}