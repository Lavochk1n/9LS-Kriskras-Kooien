using UnityEngine;
using UnityEngine.InputSystem;

namespace StackThatBulance
{
    namespace Quarantine
    {
        public class PlayerMovement : MonoBehaviour
        {
            private InputAction movementInput;
            private Vector2 moveVector;
            public float moveSpeed = 5f;
            public bool playerOne;
            private Rigidbody rb;

            private void Awake()
            {
                rb = GetComponent<Rigidbody>();

                
                //movementInput = new InputAction(binding: "<Gamepad>/leftStick");
                //movementInput.Enable();
                // movementInput.performed += ctx => OnMovementPerformed(ctx);
                // movementInput.canceled += ctx => OnMovementCancelled(ctx);

                if (!GameManager.Instance.playerOneSpawned)
                {
                    playerOne = true;
                    GameManager.Instance.playerOneSpawned = true;
                }
                else
                {
                    playerOne = false;
                }
            }

            public void OnMovementPerformed(InputAction.CallbackContext context)
            {
                moveVector = context.ReadValue<Vector2>();
            }

        

            private void FixedUpdate()
            {
                Vector3 movement = new Vector3(moveVector.x, 0f, moveVector.y);
                rb.velocity = movement * moveSpeed * Time.deltaTime;
            }
        }
    }
}
