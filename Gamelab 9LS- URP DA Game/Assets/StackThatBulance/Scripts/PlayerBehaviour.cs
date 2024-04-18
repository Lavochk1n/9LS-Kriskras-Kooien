using UnityEngine;
using UnityEngine.InputSystem;

namespace StackThatBulance
{
    public class PlayerBehaviour : Interactor
    {
        private CursorBehaviour CursorMovement;
        public bool playerOne;

        private Vector2 previousMoveVector = Vector2.zero;
        private Item currentItem;

        private void Awake()
        {
            CursorMovement = GameObject.FindWithTag("Cursor").GetComponent<CursorBehaviour>();

            if (!MiniGameManager.Instance.playerOneSpawned)
            {
                playerOne = true;
                MiniGameManager.Instance.playerOneSpawned = true;
            }
            else
            {
                playerOne = false;
            }
        }

        private void Update()
        {
            ScanInteractable(CursorMovement.gameObject, Vector3.down, 10f);
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            Vector2 moveVector = value.ReadValue<Vector2>();

            // Normalize the move vector to ensure diagonal movement is not faster
            moveVector.Normalize();

            if (playerOne)
            {
               
                moveVector.x = Mathf.Max(0, moveVector.x);
                moveVector.y = Mathf.Max(0, moveVector.y);
            }
            else
            {
               
                moveVector.x = Mathf.Min(0, moveVector.x);
                moveVector.y = Mathf.Min(0, moveVector.y);
            }

           
            CursorMovement.moveVector = new Vector3(moveVector.x, 0f, moveVector.y);
            previousMoveVector = moveVector;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                RequestInteraction();
            }
        }

        public void OnRotate(InputAction.CallbackContext context)
        {
             if (context.started)
            {
                RotateItem();
            }
        }


        private void RotateItem()
        {
            if (CursorMovement.grabTarget != null)

            {
               
             Debug.Log("Rotate button pressed");
             CursorMovement.grabTarget.transform.Rotate(Vector3.forward, 90f);
           
            }
        }
    }
}




