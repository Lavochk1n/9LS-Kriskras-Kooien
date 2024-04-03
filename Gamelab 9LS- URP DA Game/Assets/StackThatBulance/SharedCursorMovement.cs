using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SharedCursorMovement : MonoBehaviour
{
    private SharedCursor input = null;
    private Vector3 moveVector = Vector3.zero;
    private Rigidbody rb;
    
    [SerializeField]private float movespeed = 10f;


    private GameObject GrapTarget;

    private void Awake()
    {
        input = new SharedCursor();
        rb= GetComponent<Rigidbody>();  
    }

    private void OnEnable()
    {
        input.Enable();
        input.PlayerOne.HotizontalMovement.performed += OnHoriMovePerformed;
        input.PlayerTwo.VerticalMovement.performed += OnVertiMovePerformed;

        input.PlayerOne.HotizontalMovement.canceled += OnHoriMoveCancelled;
        input.PlayerTwo.VerticalMovement.canceled += OnvertiMoveCancelled;


        input.PlayerOne.Grap.performed += PlayerOneGrapPerformed;
        input.PlayerTwo.Grap.performed += PlayerTwoGrapPerformed;



    }

    private void OnDisable()
    {
        input.Disable();
        input.PlayerOne.HotizontalMovement.performed -= OnHoriMovePerformed;
        input.PlayerTwo.VerticalMovement.performed -= OnVertiMovePerformed;

        input.PlayerOne.HotizontalMovement.canceled -= OnHoriMoveCancelled;
        input.PlayerTwo.VerticalMovement.canceled -= OnvertiMoveCancelled;

        //input.PlayerOne.Grap.canceled -= PlayerOneGrapPerformed;
        //input.PlayerTwo.Grap.canceled -= PlayerTwoGrapPerformed;
    }


    private void FixedUpdate()
    {

        rb.velocity = moveVector * movespeed; 
        
        if(GrapTarget != null)
        {
            GrapTarget.transform.position = this.transform.position;
            
        }

    }

    private void OnHoriMovePerformed(InputAction.CallbackContext value)
    {
        moveVector.x = value.ReadValue<float>();
    }

    private void OnVertiMovePerformed(InputAction.CallbackContext value)
    {
        moveVector.z = value.ReadValue<float>();
    }


    private void OnHoriMoveCancelled(InputAction.CallbackContext value)
    {
        moveVector.x = 0;
    }

    private void OnvertiMoveCancelled(InputAction.CallbackContext value)
    {
        moveVector.z = 0;
    }


    private void PlayerOneGrapPerformed(InputAction.CallbackContext value)
    {
        AttemptGrap();
    }

    private void PlayerTwoGrapPerformed(InputAction.CallbackContext value)
    {
        AttemptGrap();

    }

    private void AttemptGrap()
    {
        if(GrapTarget == null)
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
