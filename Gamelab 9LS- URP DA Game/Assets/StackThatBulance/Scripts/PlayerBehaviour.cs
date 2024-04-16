using UnityEngine;
using UnityEngine.InputSystem;

namespace StackThatBulance
{
    public class PlayerBehaviour : Interactor
    {
        private CursorBehaviour CursorMovement;
        public bool playerOne;

        private Vector2 previousMoveVector = Vector2.zero;

        private void Awake()
        {
            CursorMovement = GameObject.FindWithTag("Cursor").GetComponent<CursorBehaviour>();

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

        private void Update()
        {
            ScanInteractable(CursorMovement.gameObject, Vector3.forward, 10f);
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            Vector2 moveVector = value.ReadValue<Vector2>();

            if (playerOne)
            {
                // Player One can only move up and left
                if (moveVector.x > 0 && previousMoveVector.x <= 0)
                    moveVector.x = 0;
                if (moveVector.y > 0 && previousMoveVector.y <= 0)
                    moveVector.y = 0;
            }
            else
            {
                // Player Two can only move down and right
                if (moveVector.x < 0 && previousMoveVector.x >= 0)
                    moveVector.x = 0;
                if (moveVector.y < 0 && previousMoveVector.y >= 0)
                    moveVector.y = 0;
            }

            CursorMovement.moveVector = moveVector;
            previousMoveVector = moveVector;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started)
            {

                RequestInteraction();
            }
        }
    }
}
