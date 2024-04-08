using UnityEngine;
using UnityEngine.InputSystem;


namespace Quarantine
{
    public class PlayerMovement : Interactor
    {
        private Vector2 moveVector;
        public float moveSpeed = 5f;
        //public bool playerOne;
        private Rigidbody rb;

        public animalTypes heldAnimal = animalTypes.Empty;
        public sickState heldSickState = sickState.healthy;

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


        private void Update()
        {
            ScanInteractable(gameObject, Vector3.forward, 3);
        }

        public void OnMovementPerformed(InputAction.CallbackContext context)
        {
            moveVector = context.ReadValue<Vector2>();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                RequestInteraction();
            }
        }


        private void FixedUpdate()
        {
            Vector3 movement = new Vector3(moveVector.x, 0f, moveVector.y);
            rb.velocity = movement * moveSpeed * Time.deltaTime;
        }
    }
}

