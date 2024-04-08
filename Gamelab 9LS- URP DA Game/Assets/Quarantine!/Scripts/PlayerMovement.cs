using UnityEngine;
using UnityEngine.InputSystem;


    namespace Quarantine
    {
        public class PlayerMovement : MonoBehaviour
        {
            //private InputAction movementInput;
            private Vector2 moveVector;
            public float moveSpeed = 5f;
            //public bool playerOne;
            private Rigidbody rb;

            private void Awake()
            {
                rb = GetComponent<Rigidbody>();

                // Set up movement input action
                //movementInput = new InputAction(binding: "<Gamepad>/leftStick" + "<Keyboard>/WASD" + "<Keyboard>/Arrows");
                //movementInput.Enable();
                //movementInput.performed += ctx => OnMovementPerformed(ctx);

                //if (!GameManager.Instance.playerOneSpawned)
                //{
                //    playerOne = true;
                //    GameManager.Instance.playerOneSpawned = true;
                //}
                //else
                //{
                //    playerOne = false;
                //}
            }

            public void OnMovementPerformed(InputAction.CallbackContext context)
            {
                moveVector = context.ReadValue<Vector2>();
                Debug.Log(moveVector);
            }

            private void FixedUpdate()
            {
                Vector3 movement = new Vector3(moveVector.x, 0f, moveVector.y);
                rb.velocity = movement * moveSpeed * Time.deltaTime;
            }
        }
    }

