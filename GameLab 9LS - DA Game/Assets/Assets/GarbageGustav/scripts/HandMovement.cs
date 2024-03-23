using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandMovement : MonoBehaviour
{
    private Hedgehog input = null;
    private Vector3 moveVector = Vector3.zero;
    private Rigidbody rb;

    [SerializeField] private float movespeed = 10f;

    [SerializeField] private bool playerOne;

    private GameObject GrapTarget;

    [SerializeField] float grapDistance= 10f; 


    private void Awake()
    {
        input = new Hedgehog();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        input.Enable();

        if (playerOne)
        {
            input.Player1.P1Movement.performed += HandMovementPerformed;
            input.Player1.P1Movement.canceled += HandMovementCancelled;
            input.Player1.Grap.performed += GrapPerformed;
        }
        else
        {
            input.Player2.P2Movement.performed += HandMovementPerformed;
            input.Player2.P2Movement.canceled += HandMovementCancelled;
            input.Player2.Grap.performed += GrapPerformed;
        }
       
    }

    private void OnDisable()
    {
        input.Enable();

        if (playerOne)
        {
            input.Player1.P1Movement.performed -= HandMovementPerformed;
            input.Player1.P1Movement.canceled -= HandMovementCancelled;
            input.Player1.Grap.performed -= GrapPerformed;
        }
        else
        {
            input.Player2.P2Movement.performed -= HandMovementPerformed;
            input.Player2.P2Movement.canceled -= HandMovementCancelled;
            input.Player2.Grap.performed -= GrapPerformed;
        }
    }

    private void HandMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector3>();
    }

    private void HandMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector3.zero;
    }

    private void FixedUpdate()
    {
        rb.velocity = moveVector * movespeed * Time.deltaTime;

        GrapHandeler();

        
    }

    private void GrapHandeler()
    {
        if (GrapTarget != null)
        {
            RotationButton button = GrapTarget.GetComponent<RotationButton>(); 

            if(button != null)
            {
                button.Rotate();
            }

            GrapTarget.GetComponent<Rigidbody>().velocity = moveVector * movespeed * Time.deltaTime;

        }
    }

    private void GrapPerformed(InputAction.CallbackContext value)
    {
        AttemptGrap(); 
    }

    private void AttemptGrap()
    {
        if (GrapTarget == null)
        {
            Debug.Log("Attempt Grap");

            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.forward, out hit, grapDistance))
            {
                Debug.Log(hit.transform.gameObject);
                GrapTarget = hit.transform.gameObject;

                Debug.Log("grap:" + GrapTarget);



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
